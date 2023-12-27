import { Component } from '@angular/core';
import {FormsModule} from '@angular/forms';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatButtonModule} from '@angular/material/button';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { WholesalerClass } from '../../Models/WholesalerModel';
import { BreweryClass } from '../../Models/BreweryModel';
import { BreweryInfoService } from '../../Services/brewery-info.service';
import { BreweryPostService } from '../../Services/brewery-post.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-wholesaler-edit',
  standalone: true,
  imports: [FormsModule, MatInputModule, MatSelectModule, MatFormFieldModule, MatButtonModule, MatCheckboxModule],
  templateUrl: './wholesaler-edit.component.html',
  styleUrl: './wholesaler-edit.component.css'
})
export class WholesalerEditComponent {

  isButtonDisabled: boolean = false;
  itemId: number = 0;
  breweries: BreweryClass[] = [];
  wholesaler: WholesalerClass = {
    id: 0,
    name: '',
    stockLimit: 0,
    sales: [],
    stocks: [],
    allowedBeers: []
  };

  onSubmit() {

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

    if (this.route.getCurrentNavigation()?.extras?.state != undefined){
      this.wholesaler = this.route.getCurrentNavigation()?.extras?.state!['wholesaler'];
    }
  }

}
