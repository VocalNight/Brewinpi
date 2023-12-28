import { Component } from '@angular/core';
import {FormsModule, NonNullableFormBuilder} from '@angular/forms';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatButtonModule} from '@angular/material/button';
import { WholesalerClass } from '../../Models/WholesalerModel';
import { BreweryClass } from '../../Models/BreweryModel';
import { BreweryInfoService } from '../../Services/brewery-info.service';
import { BreweryPostService } from '../../Services/brewery-post.service';
import { Router } from '@angular/router';
import { BeerClass } from '../../Models/BeersModel';

@Component({
  selector: 'app-wholesaler-edit',
  standalone: true,
  imports: [FormsModule, MatInputModule, MatSelectModule, MatFormFieldModule, MatButtonModule],
  templateUrl: './wholesaler-edit.component.html',
  styleUrl: './wholesaler-edit.component.css'
})
export class WholesalerEditComponent {

  isButtonDisabled: boolean = false;
  itemId: number = 0;
  beers: BeerClass[] = [];
  breweries: BreweryClass[] = [];
  wholesaler: WholesalerClass = {
    id: 0,
    name: '',
    stockLimit: 0,
    sales: [],
    stocks: [],
    allowedBeersId: []
  };

  onSubmit() {
    console.log(this.wholesaler);
    this.isButtonDisabled = true;

    if (this.wholesaler.id != 0) {
      this.wholesaler.sales = [];
      this.wholesaler.stocks = [];
      this.breweryPostService.updateItem(this.wholesaler, 'Wholesalers');
    } else {
      this.breweryPostService.postItem(this.wholesaler, 'Wholesalers', this.route);
    }
  }

  constructor(private breweryInfoService: BreweryInfoService, private breweryPostService: BreweryPostService, private route: Router) {
    this.getBeers();
    if (this.route.getCurrentNavigation()?.extras?.state != undefined){
      this.wholesaler = this.route.getCurrentNavigation()?.extras?.state!['wholesaler'];
    }
    console.log(this.wholesaler);
  }
  
  getBeers() {
    this.breweryInfoService.getInfo('Beers').subscribe((result) => {
      this.beers = result;
    }); 
  }
}
