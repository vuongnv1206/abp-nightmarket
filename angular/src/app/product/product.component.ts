
import { PagedResultDto } from '@abp/ng.core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';
import { ProductInListDto, ProductService } from '@proxy/products';
import { Subject, takeUntil } from 'rxjs';
import { ProductCategoryInListDto, ProductCategoryService } from '@proxy/product-categories';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent implements OnInit ,OnDestroy{

  private ngUnsubscribe = new Subject<void>();
  blockedPanel:boolean = false;
  products: ProductInListDto[] = [];

  //Paging variables
  public maxResultCount: number = 10;
  public skipCount: number = 0;
  public totalCount: number;

  //Filter variables
  productCategories : any[] = [];
  keyword:string = '';
  categoryId:string = '';

  //Constructor
  constructor(private authService: AuthService,
    private oAuthService: OAuthService,
    private productService : ProductService,
    private productCategoryService: ProductCategoryService

    ) {}

  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  login() {
    this.authService.navigateToLogin();
  }



    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }

  ngOnInit(): void {
    this.loadProductCategories();
    this.loadData();
  }


  loadData(){
    this.productService.getListWithFilter({
      keyWord: this.keyword,
      categoryId: this.categoryId,
      maxResultCount: this.maxResultCount,
      skipCount: this.skipCount,
    })
    .pipe(takeUntil(this.ngUnsubscribe))
    .subscribe({
      next : (response : PagedResultDto<ProductInListDto>) => {
        this.products = response.items;
        this.totalCount = response.totalCount;
      },
      error: () =>{

      },
    });
  }


  loadProductCategories(){
    this.productCategoryService.getListAll()
    .subscribe((response : ProductCategoryInListDto[]) => {
      response.forEach(category =>{
        this.productCategories.push({
          name: category.name,
          value: category.id,
        });
      });
    });
  }

  onPageChange(event: any): void{
    this.skipCount = (event.page -1) * this.maxResultCount;
    this.maxResultCount = event.rows;
    this.loadData();
  }

}
