import { Component } from '@angular/core';
import {FormsModule} from '@angular/forms';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatButtonModule} from '@angular/material/button';
import { SalesClass } from '../../Models/SalesModel';
import { BreweryInfoService } from '../../Services/brewery-info.service';
import { BreweryPostService } from '../../Services/brewery-post.service';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { BeerClass } from '../../Models/BeersModel';
import { WholesalerClass } from '../../Models/WholesalerModel';

@Component({
  selector: 'app-sale-edit',
  standalone: true,
  imports: [FormsModule, MatInputModule, MatSelectModule, MatFormFieldModule, MatButtonModule],
  templateUrl: './sale-edit.component.html',
  styleUrl: './sale-edit.component.css'
})
export class SaleEditComponent {
  isButtonDisabled: boolean = false;
  itemId: number = 0;
  beers: BeerClass[] = [];
  wholesalers: WholesalerClass[] = [];
  sale: SalesClass = {
    id: 0,
    breweryId: 0,
    wholesalerId: 0,
    beerId: 0,
    quantity: 0,
    saleDate: new Date()
  };

  onSubmit() {

    this.isButtonDisabled = true;
    this.breweryPostService.postItem(this.sale, 'Breweries');
      setTimeout(() => 
    {
      this.route.navigate([`Breweries`]);
    },
      1000);
    }
  
  constructor(private breweryInfoService: BreweryInfoService, private breweryPostService: BreweryPostService, private route: Router) {
    this.getBeers();
    this.getWholesalers();
  }

  getBeers() {
    this.breweryInfoService.getInfo('Beers').subscribe((result) => {
      this.beers = result;
    });
  }

  getWholesalers() {
    this.breweryInfoService.getInfo('Wholesalers').subscribe((result) => {
      this.wholesalers = result;
    });
  }

  getQuote() {
    this.breweryInfoService.getQuote(this.sale.beerId, this.sale.quantity, this.sale.wholesalerId)
      .subscribe((result) => {
        console.log(result)
      });
  }
}
