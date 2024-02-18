import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
} from "@angular/core";

@Component({
  selector: "app-paginator",
  templateUrl: "./paginator.component.html",
})
export class PaginatorComponent implements OnChanges {
  @Input({ required: true }) totalPages: number;
  @Input({ required: true }) pageNumber: number;
  @Input({ required: true }) hasPreviousPage: boolean;
  @Input({ required: true }) hasNextPage: boolean;

  private readonly totalPagesToShow = 4;

  @Output() pageChange: EventEmitter<number> = new EventEmitter<number>();

  protected pages: number[] = [];

  ngOnChanges(): void {
    console.log(this.pageNumber, "currentPage");
    console.log(this.hasPreviousPage, "hasPreviousPage");
    console.log(this.hasNextPage, "hasNextPage");

    this.generatePageNumbers();
  }

  protected goToPrevPage(): void {
    this.pageNumber--;
    this.goToPage(this.pageNumber);
  }

  protected goToNextPage(): void {
    this.pageNumber++;
    this.goToPage(this.pageNumber);
  }

  protected goToPage(page: number | null): void {
    if (page) {
      this.pageNumber = page;
    }
    this.onPageChange();
  }

  private onPageChange(): void {
    this.pageChange.emit(this.pageNumber);
  }

  private generatePageNumbers(): void {
    let startPage = Math.max(1, this.pageNumber - 1);
    let endPage = Math.min(
      startPage + this.totalPagesToShow - 1,
      this.totalPages
    );

    this.pages = [];
    for (let i = startPage; i <= endPage; i++) {
      this.pages.push(i);
    }

    while (
      this.pages.length < this.totalPagesToShow &&
      endPage < this.totalPages
    ) {
      this.pages.push(++endPage);
    }

    while (this.pages.length < this.totalPagesToShow && startPage > 1) {
      this.pages.unshift(--startPage);
    }
  }
}
