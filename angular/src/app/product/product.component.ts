import { PagedResultDto } from '@abp/ng.core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';
import { ProductDto, ProductInListDto, ProductService } from '@proxy/products';
import { Subject, takeUntil } from 'rxjs';
import { ProductCategoryInListDto, ProductCategoryService } from '@proxy/product-categories';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { NotificationService } from '../shared/services/notification.service';
import { ProductType } from '@proxy/night-market/products';
import { ConfirmationService } from 'primeng/api';
import { ProductAttributeComponent } from './product-attribute/product-attribute.component';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
})
export class ProductComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel = false;
  products: ProductInListDto[] = [];
  product: ProductDto;
  selectedProducts: ProductInListDto[] = [];
  public thumbnailImage;



  //Paging variables
  public maxResultCount: number = 10;
  public skipCount: number = 0;
  public totalCount: number;

  //Filter variables
  productCategories: any[] = [];
  keyWord: string = '';
  categoryId: string = '';

  //Constructor
  constructor(
    private productService: ProductService,
    private productCategoryService: ProductCategoryService,
    private dialogService: DialogService,
    private notificationService: NotificationService,
    private confirmmationService: ConfirmationService,
    private sanitizer: DomSanitizer,
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit(): void {
    this.loadProductCategories();
    this.loadData();
  }

  loadData() {
    this.toggleBlockUI(true);
    this.productService
      .getListWithFilter({
        keyWord: this.keyWord,
        categoryId: this.categoryId,
        maxResultCount: this.maxResultCount,
        skipCount: this.skipCount,
      })
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: PagedResultDto<ProductInListDto>) => {
          this.products = response.items;
          // for (let item of this.products) {
          //   this.loadThumbnail(item.thumbnailPicture, item);
          // }
          this.totalCount = response.totalCount;
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
          label: category.name,
          value: category.id,
        });
      });
    });
  }

  getProductTypeName(value: number) {
    return ProductType[value];
  }

  onPageChange(event: any): void {
    this.skipCount = (event.page - 1) * this.maxResultCount;
    this.maxResultCount = event.rows;
    this.loadData();
  }

  openNew() {
    const newProduct: ProductDto = {
      productType: ProductType.Single,
      visibility: true,
      isActive: true,
      sellPrice: 0
    };
    const ref = this.dialogService.open(ProductDetailComponent, {
      header: 'Add Product',
      width: '40%',
      data: newProduct,
      modal: true,
      styleClass:'p-fluid'

    });
    ref.onClose.subscribe((data: ProductDto) => {
      if (data) {
        this.loadData();
        this.notificationService.showSuccess('Add new product successfully');
        this.selectedProducts = [];
      }
    });
  }


  public showEditModalGlobal() {
    if (this.selectedProducts.length == 0) {
      this.notificationService.showError('Select one product !');
      return;
    }
    const id = this.selectedProducts[0].id;
    const ref = this.dialogService.open(ProductDetailComponent, {
      data: { id: id },
      width: '40%',
      header: 'Update Product',
      modal: true,
      styleClass:'p-fluid'
    });


    ref.onClose.subscribe((data: ProductDto) => {
      if (data) {
        this.loadData();
        this.selectedProducts = [];
        this.notificationService.showSuccess('Update product successfully');
      }
    });
  }

  editProduct(product: ProductInListDto) {
    this.product = { ...product };
    const ref = this.dialogService.open(ProductDetailComponent, {
      data: { id: this.product.id},
      width: '40%',
      header: 'Update Product',
      modal: true,
      styleClass:'p-fluid'

    });
    ref.onClose.subscribe((data: ProductDto) => {
      if (data) {
        this.loadData();
        this.notificationService.showSuccess('Update product successfully');
      }
    });

}

  manageProductAttribute(id: string) {
    const ref = this.dialogService.open(ProductAttributeComponent, {
      data: {
        id: id,
      },
      header: 'Quản lý thuộc tính sản phẩm',
      width: '40%',
    });

    ref.onClose.subscribe((data: ProductDto) => {
      if (data) {
        this.loadData();
        this.selectedProducts = [];
        this.notificationService.showSuccess('Cập nhật thuộc tính sản phẩm thành công');
      }
    });
  }

  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      this.blockedPanel = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
      }, 1000);
    }
  }

  deleteItems() {
    if (this.selectedProducts.length == 0) {
      this.notificationService.showError('Must be at least one item');
      return;
    }
    var ids = [];
    this.selectedProducts.forEach(element => {
      ids.push(element.id);
    });
    this.confirmmationService.confirm({
      message: 'Are you sure you want to delete this item',
      accept: () => {
        this.deleteItemsConfirmed(ids);
      },
    });
  }

  deleteProduct(product: ProductDto) {
    this.confirmmationService.confirm({
        message: 'Are you sure you want to delete ' + product.name + '?',
        header: 'Confirm',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          this.deleteItemsConfirmed([product.id]);
        }
    });
}

  deleteItemsConfirmed(ids: string[]) {
    this.toggleBlockUI(true);
    this.productService
      .deleteMultiple(ids)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.notificationService.showSuccess('Delete successfully completed');
          this.loadData();
          this.selectedProducts = [];
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }


  loadThumbnail(fileName: string, item: ProductInListDto) {
    this.productService
      .getThumbnailImage(fileName)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: string) => {
          const fileExt = fileName.split('.').pop();
          const imageUrl = `data:image/${fileExt};base64, ${response}`;
          item.thumbnailPicture = imageUrl; // Không sử dụng bypassSecurityTrustUrl ở đây
        },
      });
  }

  // loadThumbnail(fileName: string) {
  //   this.productService
  //     .getThumbnailImage(fileName)
  //     .pipe(takeUntil(this.ngUnsubscribe))
  //     .subscribe({
  //       next: (response: string) => {
  //         var fileExt = this.selectedEntity.thumbnailPicture?.split('.').pop();
  //         this.thumbnailImage = this.sanitizer.bypassSecurityTrustResourceUrl(
  //           `data:image/${fileExt};base64, ${response}`
  //         );
  //       },
  //     });
  // }





}
