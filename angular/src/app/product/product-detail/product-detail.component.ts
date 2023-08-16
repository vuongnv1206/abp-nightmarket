import { PagedResultDto } from '@abp/ng.core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ProductDto, ProductInListDto, ProductService } from '@proxy/products';
import { Subject, takeUntil } from 'rxjs';
import { ProductCategoryInListDto, ProductCategoryService } from '@proxy/product-categories';
import { AuthService } from 'src/app/shared/services/auth.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss'],
})
export class ProductDetailComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel: boolean = false;
  btnDisabled:boolean = false;

  //Dropdowm
  productCategories: any[] = [];
  selectedEntity ={} as ProductDto;
  manufacturers:any[] = [];
  categories: any[] = [];
  productTypes: any[] = [];

  public form: FormGroup;

  //Constructor
  constructor(
    private authService: AuthService,
    private oAuthService: OAuthService,
    private productService: ProductService,
    private productCategoryService: ProductCategoryService,
    private fb: FormBuilder
  ) {}


  validationMessages = {
    code:[{
      type:'required',
      message:'You must enter unique code'
    }],
    name:[
      { type:'required',message:'Field name is required' },
      { type: 'maxlength', message: 'You must not enter more than 250 characters' },
    ],
    slug:[
      {type:'required',message:'Field slug is required'}
    ],
    sku:[{type:'required',message:'Field slug is required and unique'}],
    manufacturerId:[{type:'required',message:'Manufacturer is required'}],
    categoryId:[{type:'required',message:'Manufacturer is required'}],
    productType:[{type:'required',message:'ProductType is required'}],
    sortOrder:[{type:'required',message:'SortOrder is required'}],
    sellPrice:[{type:'required',message:'SellPrice is required'}],

  }

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
    this.buildForm();
  }

  loadFormDetails(id: string) {
    this.toggleBlockUI(true);
    this.productService
      .get(id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: ProductDto) => {
          this.selectedEntity = response;
          this.buildForm();
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  loadProductCategories() {
    this.productCategoryService.getListAll().subscribe((response: ProductCategoryInListDto[]) => {
      response.forEach(category => {
        this.productCategories.push({
          name: category.name,
          value: category.id,
        });
      });
    });
  }

  private buildForm() {
    this.form = this.fb.group({
      name: new FormControl(this.selectedEntity.name || null, Validators.compose([
        Validators.required,
        Validators.maxLength(250)
      ])),
      code: new FormControl(this.selectedEntity.code || null, Validators.required),
      slug: new FormControl(this.selectedEntity.slug || null, Validators.required),
      sku: new FormControl(this.selectedEntity.sku || null, Validators.required),
      productType: new FormControl(this.selectedEntity.productType || null, Validators.required),
      manufacturerId: new FormControl(this.selectedEntity.manufacturerId || null, Validators.required),
      categoryId: new FormControl(this.selectedEntity.categoryId || null, Validators.required),
      sellPrice: new FormControl(this.selectedEntity.sellPrice || null, Validators.required),
      sortOrder: new FormControl(this.selectedEntity.sortOrder || null, Validators.required),
      visibility: new FormControl(this.selectedEntity.visibility || true),
      isActive: new FormControl(this.selectedEntity.isActive || true),
      desciption: new FormControl(this.selectedEntity.description || null),
      seoMetaDescription: new FormControl(this.selectedEntity.seoMetaDescription || null),
      thumbnailPicture: new FormControl(this.selectedEntity.thumbnailPicture || null, Validators.required),

    });
  }

  private toggleBlockUI(enable: boolean) {
    if (enable == true) {
      this.blockedPanel = true;
      this.btnDisabled = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
        this.btnDisabled = false;
      }, 1000);
    }
  }

   saveChange() {}
}
