import { PagedResultDto } from '@abp/ng.core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';
import { ProductDto, ProductInListDto, ProductService } from '@proxy/products';
import { Subject, takeUntil } from 'rxjs';
import { ProductCategoryInListDto, ProductCategoryService } from '@proxy/product-categories';
import { DialogService } from 'primeng/dynamicdialog';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { NotificationService } from '../shared/services/notification.service';
import { ProductType } from '@proxy/night-market/products';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
})
export class ProductComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel = false;
  items: ProductInListDto[] = [];
  public selectedItems: ProductInListDto[] = [];

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
    private confirmmationService : ConfirmationService
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
          this.items = response.items;
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

  public showAddModal() {
    const ref = this.dialogService.open(ProductDetailComponent, {
      header: 'Add Product',
      width: '70%',
    });

    ref.onClose.subscribe((data: ProductDto) => {
      if (data) {
        this.loadData();
        this.notificationService.showSuccess('Add new product successfully');
        this.selectedItems = [];
      }
    });
  }

  public showEditModal() {
    if (this.selectedItems.length == 0) {
      this.notificationService.showError('Choose atleast one product');
      return;
    }
    const id = this.selectedItems[0].id;
    const ref = this.dialogService.open(ProductDetailComponent, {
      data: { id: id },
      header: 'Update Product',
      width: '70%',
    });

    ref.onClose.subscribe((data: ProductDto) => {
      if (data) {
        this.loadData();
        this.selectedItems = [];
        this.notificationService.showSuccess('Update product successfully');
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

  deleteItems(){
    if(this.selectedItems.length == 0){
      this.notificationService.showError("Must be at least one item");
      return;
    }
    var ids = [];
    this.selectedItems.forEach(element => {
      ids.push(element.id);
    });
    this.confirmmationService.confirm({
      message: "Are you sure you want to delete this item",
      accept:() => {
        this.deleteItemsConfirmed(ids);
      }
    });
  }

  deleteItemsConfirmed(ids: string[]){
    this.toggleBlockUI(true);
    this.productService.deleteMultiple(ids)
    .pipe(takeUntil(this.ngUnsubscribe))
    .subscribe({
      next:() =>{
        this.notificationService.showSuccess("Delete successfully completed");
        this.loadData();
        this.selectedItems = [];
        this.toggleBlockUI(false);
      },
      error:() =>{
        this.toggleBlockUI(false);
      },
    });
  }
}
