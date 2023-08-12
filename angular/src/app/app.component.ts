import { Component, OnInit } from '@angular/core';
import { PrimeNGConfig } from 'primeng/api';

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
  constructor(private primeNgConfig : PrimeNGConfig){

  }
  ngOnInit(): void {
    this.primeNgConfig.ripple = true;
    document.documentElement.style.fontSize = '14px';
  }

}
