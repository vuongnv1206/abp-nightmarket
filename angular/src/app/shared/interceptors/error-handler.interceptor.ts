import { Injectable } from '@angular/core';
import {HttpRequest,HttpHandler,HttpEvent,HttpInterceptor,}from '@angular/common/http';
import {catchError,Observable} from 'rxjs';
import { NotificationService } from '../services/notification.service';

@Injectable()
export class GlobalHttpInterceptorService implements HttpInterceptor {
  constructor(private notificationservice: NotificationService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(ex => {
        if (ex.status == 500) {
          this.notificationservice.showError('Hệ thống có lôi xảy ra. Vui lòng liên hệ admin');
        }
        throw ex;
      })
    );
  }
}
