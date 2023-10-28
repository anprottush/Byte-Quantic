import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    private authToken = 'your_auth_token'; // Replace with your actual token

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (this.authToken) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${this.authToken}`
                }
            });
        }
        return next.handle(request);
    }
}
