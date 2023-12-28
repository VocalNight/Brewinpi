import { Component } from '@angular/core';
import { WholesalerClass } from '../Models/WholesalerModel';
import { BreweryInfoService } from '../Services/brewery-info.service';
import {MatTableModule} from '@angular/material/table';
import {MatDividerModule} from '@angular/material/divider';
import {MatButtonModule} from '@angular/material/button';
import { BreweryPostService } from '../Services/brewery-post.service';
import { Router, RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-wholesaler-view',
  standalone: true,
  imports: [RouterLink, RouterOutlet, MatTableModule, MatDividerModule, MatButtonModule],
  templateUrl: './wholesaler-view.component.html',
  styleUrl: './wholesaler-view.component.css'
})
export class WholesalerViewComponent {
  wholesalers: WholesalerClass[] = [];
  columnsToDisplay = ['id', 'name', 'stock', 'beers'];
  clickedWholesaler!: WholesalerClass;

  constructor(private breweryInfoService: BreweryInfoService, private breweryPostService: BreweryPostService, private router: Router) {
    this.getWholesalers();
    console.log(this.wholesalers);
  } 

  deleteBrewery() {

    if (this.clickedWholesaler) {
      this.breweryPostService.deleteRow(this.clickedWholesaler.id, 'Wholesalers');
      this.wholesalers = this.wholesalers.filter(b => b.id !== this.clickedWholesaler.id)  
    }
  }

  editItem() {
    if (this.clickedWholesaler) {
      let wholesaler = this.clickedWholesaler;
      this.router.navigate([`Wholesalers/edit`], { state: {wholesaler}});
    } 
  }

  getCurrentStock(wholesaler: WholesalerClass) {
    return wholesaler.stocks?.reduce((total, stock) => total + stock.stockQuantity, 0)
  }

  getWholesalers() {
    this.breweryInfoService.getInfo('Wholesalers').subscribe((result) => {
      this.wholesalers = result;
      console.log(this.wholesalers);
    });
  }
}
