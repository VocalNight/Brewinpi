import { Component } from '@angular/core';
import { SalesClass } from '../Models/SalesModel';
import {MatTableModule} from '@angular/material/table';
import {MatDividerModule} from '@angular/material/divider';
import {MatButtonModule} from '@angular/material/button';
import { BreweryInfoService } from '../Services/brewery-info.service';
import { BreweryPostService } from '../Services/brewery-post.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sales-view',
  standalone: true,
  imports: [MatButtonModule, MatDividerModule, MatTableModule],
  templateUrl: './sales-view.component.html',
  styleUrl: './sales-view.component.css'
})
export class SalesViewComponent {
  sales: SalesClass[] = [];
  columnsToDisplay = ['id', 'quantity', 'date'];
  clickedSale!: SalesClass;

  constructor(private breweryInfoService: BreweryInfoService, private breweryPostService: BreweryPostService, private router: Router) {
   this.getBeers();
  }
  
  deleteBeer() {

    if (this.clickedSale) {
      this.breweryPostService.deleteRow(this.clickedSale.id, 'Sales');
      this.sales = this.sales.filter(s => s.id !== this.clickedSale.id)  
    } else {

    }
  }

  editItem() {
    if (this.clickedSale) {
      let beer = this.clickedSale;
      this.router.navigate([`Sales/edit`], { state: {beer}});
    }
    
  }

  getBeers() {
    this.breweryInfoService.getInfo('Sales').subscribe((result) => {
      this.sales = result;
    });
  }

}
