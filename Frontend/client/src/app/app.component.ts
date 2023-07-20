import { AfterViewInit, Component } from '@angular/core';
import { HttpClient, HttpClientJsonpModule, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './shared/services/authService';

declare var google: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent implements AfterViewInit {
  title = 'Angular13, DotNet Core 6 API, Google Auth';
  constructor(private http: HttpClient, public authService: AuthService) { 
    this.authService.checkToken();
  }

  ngAfterViewInit(): void {
    google.accounts.id.initialize({
      client_id: '493860766794-llv1fkopcb3ag4vk8fara82jtagger1p.apps.googleusercontent.com', // Your client id
      callback: this.authService.handleCredentialResponse.bind(this.authService)
    });
    google.accounts.id.renderButton(
      document.getElementById("buttonDiv"), // Points to the button in the front end
      { size: "large", type: "icon", shape: "pill" } // Change the Google button's look
    );

  }
}
