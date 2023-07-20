import { HttpHeaders } from "@angular/common/http";

export class UserEntity {
    // JwtBearer AuthToken
    authToken: string | undefined;

    // GoogleToken Properties
    email: string | undefined;
    family_name: string | undefined;
    given_name: string | undefined;
    name: string | undefined;
    picture: string | undefined;

    // Authenticated
    isAuthenticated: boolean = false;

    decodeToken(token: any) {
        let base64Url = token.split('.')[1];
        let base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        let jsonPayload = JSON.parse(decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join('')));

        this.email = jsonPayload.email;
        this.family_name = jsonPayload.family_name;
        this.given_name = jsonPayload.given_name;
        this.name = jsonPayload.name;
        this.picture = jsonPayload.picture;

        // console.log(jsonPayload);
    }

    getHeaders(): HttpHeaders {
        let header = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${this.authToken}`
        });
        return header;
    }
}