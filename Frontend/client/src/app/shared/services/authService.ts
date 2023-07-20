import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserEntity } from '../models/UserEntity';

@Injectable()
export class AuthService {
    public user: UserEntity = new UserEntity;

    constructor(private http: HttpClient) { }

    handleCredentialResponse(response: any) {
        // console.log(response.credential); // This is the Google Token
        this.user.decodeToken(response.credential); // Decodes the token and gives all the information contained

        // Do what you want with the token
        this.authenticate(response.credential);
    }

    authenticate(token: any) {
        this.http.post<any>(`https://localhost:7240/user/authenticate`, { idToken: token }).subscribe((jwtToken: any) => { // Authenticate against our JwtBearer
            // localStorage.setItem('jwtToken', JSON.stringify(jwtToken)); // Store the JwtToken in the LocalStorage for later use
            this.user.authToken = jwtToken.authToken;
            this.user.isAuthenticated = true;
            localStorage.setItem("user", JSON.stringify(this.user)); // Store the User in the LocalStorage

            // Testing the AuthToken
            // this.testAuthenticated().subscribe(data => { console.log("Successful Request"); console.log(data); }); // Test the authentication if you want
            // this.testNotAuthenticated().subscribe(data => { }, error => { console.log("401 Request"); console.log(error); }); // Test the authentication if you want

            window.location.reload();
        });
    }

    testAuthenticated(): Observable<any> {
        return this.http.get<any>(`https://localhost:7240/user/test-authentication`, { headers: this.user.getHeaders() });
    }

    testNotAuthenticated(): Observable<any> {
        return this.http.get<any>(`https://localhost:7240/user/test-authentication`);
    }

    checkToken() {
        let user = localStorage.getItem("user");
        if (user) {
            this.user = JSON.parse(user) as UserEntity;
        }
    }

    signOut() {
        this.user = new UserEntity;
        localStorage.removeItem("user");
        window.location.reload();
    }
}