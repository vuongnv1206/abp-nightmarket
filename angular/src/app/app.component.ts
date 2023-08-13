import { Component, OnInit } from '@angular/core';
import { PrimeNGConfig } from 'primeng/api';
import { AuthService } from './shared/services/auth.service';
import { Router } from '@angular/router';
import { LOGIN_URL } from './shared/constants/url.const';

@Component({
  selector: 'app-root',
  template: `
    <!-- <abp-loader-bar></abp-loader-bar> -->
    <!-- <abp-dynamic-layout></abp-dynamic-layout> -->
    <router-outlet></router-outlet>
  `,
})
export class AppComponent implements OnInit {
  menuMode = 'static';
  constructor(
    private primeNgConfig : PrimeNGConfig,
    private authService : AuthService,
    private router: Router
    ){

  }
  ngOnInit(): void {
    this.primeNgConfig.ripple = true;
    document.documentElement.style.fontSize = '14px';

    if (this.authService.isAuthenticated() == false) {
      this.router.navigate([LOGIN_URL]);
    }

  };
}
