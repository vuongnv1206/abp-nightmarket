import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { ProductDto, ProductInListDto, ProductService } from '@proxy/catalogs/products';
import { Subject, forkJoin, takeUntil } from 'rxjs';
import { ProductCategoryInListDto, ProductCategoryService } from '@proxy/catalogs/product-categories';
import { ManufacturerInListDto, ManufacturerService } from '@proxy/catalogs/manufacturers';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { UtilityService } from 'src/app/shared/services/utility.service';
import { ProductType, productTypeOptions } from '@proxy/night-market//catalogs/products';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { DomSanitizer } from '@angular/platform-browser';
import { Dialog } from 'primeng/dialog';


@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss'],
})
export class ProductDetailComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel: boolean = false;
  btnDisabled = false;
  product = {} as ProductDto;
  selectProduct = {} as ProductDto;
  submitted: boolean = false;

  //Dropdowm
  productCategories: any[] = [];
  selectedEntity = {} as ProductDto;
  manufacturers: any[] = [];
  productTypes: any[] = [];
  public thumbnailImage;

  //Constructor
  constructor(
    private productService: ProductService,
    private productCategoryService: ProductCategoryService,
    private manufacturerService: ManufacturerService,
    private config: DynamicDialogConfig,
    private ref: DynamicDialogRef,
    private utilityService: UtilityService,
    private notificationService: NotificationService,
    private cd: ChangeDetectorRef,
    private sanitizer: DomSanitizer,
  ) {}

  ngOnDestroy(): void {
    if (this.ref) {
      this.ref.close();
    }
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit(): void {
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
          //Push category data
          productCategories.forEach(element => {
            this.productCategories.push({
              value: element.id,
              label: element.name,
            });
          });
          //Push manufacturer data
          manufacturers.forEach(element => {
            this.manufacturers.push({
              value: element.id,
              label: element.name,
            });
          });
          //Load add data

          if (this.utilityService.isEmpty(this.config.data?.id) == true) {
            this.product = this.config.data;
            this.getSuggestNewCode();
            this.toggleBlockUI(false);
          } else {
            //Load Edit data
            this.toggleBlockUI(true);
            const productId = this.config.data?.id;
            this.productService
              .get(productId)
              .pipe(takeUntil(this.ngUnsubscribe))
              .subscribe({
                next: (response: ProductDto) => {
                  this.product = response;
                  this.toggleBlockUI(false);
                },
                error: err => {
                  this.notificationService.showError(err.error.error.message);
                  this.toggleBlockUI(false);
                },
              });
          }
        },
        error: err => {
          this.notificationService.showError(err.error.error.message);
          this.toggleBlockUI(false);
        },
      });
  }

  generateSlug() {
    this.product.slug = this.utilityService.MakeSeoTitle(this.product.name);
  }



  getSuggestNewCode() {
    this.productService
      .getSuggestNewCode()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: string) => {
          this.product.code = response; // Set the 'code' property directly in the 'product' object
        },
      });
  }

    loadProductTypes() {
      productTypeOptions.forEach(productType => {
        this.productTypes.push({
          label: productType.key,
          value: productType.value,
        });
      });
    }

    getProductTypeName(value: number) {
      return ProductType[value];
    }

  getSeverity(status: string) {
    switch (status) {
        case 'Single':
            return 'success';

        case 'Grouped':
            return 'danger';

        case 'Configurable':
            return 'primary';

        case 'Bundle':
            return 'warning';

        case 'Virtual':
            return 'info';

        case 'Dowloadable':
            return null;
    }
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
    this.submitted = true;
    this.toggleBlockUI(true);
    //Create a new product
    if (this.utilityService.isEmpty(this.config.data?.id) == true) {
      this.productService
        .create(this.product)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: () => {
            this.ref.close(this.product);
            this.toggleBlockUI(false);
          },
          error: err => {
            this.notificationService.showError(err.error.error.message);
            this.toggleBlockUI(false);
          },
        });
    } else {
      //Update a product
      this.productService
        .update(this.config.data?.id, this.product)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: () => {
            this.toggleBlockUI(false);
            this.ref.close(this.product);
          },
          error: err => {
            this.notificationService.showError(err.error.error.message);
            this.toggleBlockUI(false);
          },
        });
    }
  }

  loadThumbnail(fileName: string) {
    this.productService
      .getThumbnailImage(fileName)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: string) => {
          var fileExt = this.selectedEntity.thumbnailPicture?.split('.').pop();
          this.thumbnailImage = this.sanitizer.bypassSecurityTrustResourceUrl(
            `data:image/${fileExt};base64, ${response}`
          );
        },
      });
  }

  hideDialog() {
    this.ref.close();
    this.submitted = false;
  }



  // onFileChange(event) {
  //   const reader = new FileReader();

  //   if (event.target.files && event.target.files.length) {
  //     const [file] = event.target.files;
  //     reader.readAsDataURL(file);
  //     reader.onload = () => {
  //       this.product.patchValue({
  //         thumbnailPictureName: file.name,
  //         thumbnailPictureContent: reader.result,
  //       });

  //       // need to run CD since file load runs outside of zone
  //       this.cd.markForCheck();
  //     };
  //   }
  // }
}
