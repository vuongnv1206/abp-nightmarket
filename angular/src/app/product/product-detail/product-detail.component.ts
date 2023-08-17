import { PagedResultDto } from '@abp/ng.core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ProductDto, ProductInListDto, ProductService } from '@proxy/products';
import { Subject, forkJoin, takeUntil } from 'rxjs';
import { ProductCategoryInListDto, ProductCategoryService } from '@proxy/product-categories';
import { AuthService } from 'src/app/shared/services/auth.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ManufacturerInListDto, ManufacturerService } from '@proxy/manufacturers';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { UtilityService } from 'src/app/shared/services/utility.service';
import { productTypeOptions } from '@proxy/night-market/products';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss'],
})
export class ProductDetailComponent implements OnInit, OnDestroy {

  private ngUnsubscribe = new Subject<void>();
  blockedPanel: boolean = false;
  btnDisabled = false;

  //Dropdowm
  productCategories: any[] = [];
  selectedEntity ={} as ProductDto;
  manufacturers:any[] = [];
  productTypes: any[] = [];

  public form: FormGroup;

  //Constructor
  constructor(
    private authService: AuthService,
    private oAuthService: OAuthService,
    private productService: ProductService,
    private productCategoryService: ProductCategoryService,
    private manufacturerService: ManufacturerService,
    private fb: FormBuilder,
    private config:DynamicDialogConfig,
    private ref:DynamicDialogRef,
    private utilityService: UtilityService
  ) {}


  validationMessages = {
    code: [{ type: 'required', message: 'Bạn phải nhập mã duy nhất' }],
    name: [
      { type: 'required', message: 'Bạn phải nhập tên' },
      { type: 'maxlength', message: 'Bạn không được nhập quá 255 kí tự' },
    ],
    slug: [{ type: 'required', message: 'Bạn phải URL duy nhất' }],
    sku: [{ type: 'required', message: 'Bạn phải mã SKU sản phẩm' }],
    manufacturerId: [{ type: 'required', message: 'Bạn phải chọn nhà cung cấp' }],
    categoryId: [{ type: 'required', message: 'Bạn phải chọn danh mục' }],
    productType: [{ type: 'required', message: 'Bạn phải chọn loại sản phẩm' }],
    sortOrder: [{ type: 'required', message: 'Bạn phải nhập thứ tự' }],
    sellPrice: [{ type: 'required', message: 'Bạn phải nhập giá bán' }],
  };

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
    this.loadProductTypes();
    this.initFormData();
  }

  initFormData() {
    //Load data to form
    var productCategories = this.productCategoryService.getListAll();
    var manufacturers = this.manufacturerService.getListAll();
    this.toggleBlockUI(true);
    forkJoin({
      productCategories,
      manufacturers,
    })
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: any) => {
          //Push data to dropdown
          var productCategories = response.productCategories as ProductCategoryInListDto[];
          var manufacturers = response.manufacturers as ManufacturerInListDto[];
          productCategories.forEach(element => {
            this.productCategories.push({
              value: element.id,
              label: element.name,
            });
          });

          manufacturers.forEach(element => {
            this.manufacturers.push({
              value: element.id,
              label: element.name,
            });
          });
          //Load edit data to form
          if (this.utilityService.isEmpty(this.config.data?.id) == true) {

            this.toggleBlockUI(false);
          } else {
            this.loadFormDetails(this.config.data?.id);
          }
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  generateSlug() {
    this.form.controls['slug'].setValue(this.utilityService.MakeSeoTitle(this.form.get('name').value));
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

  loadProductTypes() {
      productTypeOptions.forEach((productType) => {
        this.productTypes.push({
          label: productType.key,
          value: productType.value,
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
      visibility: new FormControl(this.selectedEntity.visibility),
      isActive: new FormControl(this.selectedEntity.isActive),
      description: new FormControl(this.selectedEntity.description || null),
      seoMetaDescription: new FormControl(this.selectedEntity.seoMetaDescription || null),
      thumbnailPicture: new FormControl(this.selectedEntity.thumbnailPicture || null),
    });
  }

  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      this.blockedPanel = true;
      this.btnDisabled = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
        this.btnDisabled = false;
      }, 1000);
    }
  }

   saveChange() {
    this.toggleBlockUI(true);
    if(this.utilityService.isEmpty(this.config.data?.id) == true) {
      this.productService.create(this.form.value)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.ref.close(this.form.value);
          this.toggleBlockUI(false);
        },
        error:() =>{
          this.toggleBlockUI(false);
        }
      });
    }else{
      this.productService.update(this.config.data?.id,this.form.value)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.toggleBlockUI(false);
          this.ref.close(this.form.value);
        },
        error:() =>{
          this.toggleBlockUI(false);
        }
      });
    }
}
}
