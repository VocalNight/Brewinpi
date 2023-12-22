import { Component} from '@angular/core';
import {FormsModule} from '@angular/forms';
import { Router } from '@angular/router';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatButtonModule} from '@angular/material/button';
import {MatSnackBar} from '@angular/material/snack-bar';
import { BreweryClass } from '../../Models/BreweryModel';
import { BreweryInfoService } from '../../Services/brewery-info.service';
import { BreweryPostService } from '../../Services/brewery-post.service';
import { BeerClass } from '../../Models/BeersModel';



@Component({
  selector: 'app-brew-edit',
  standalone: true,
  imports: [FormsModule, MatInputModule, MatSelectModule, MatFormFieldModule, MatButtonModule],
  templateUrl: './brew-edit.component.html',
  styleUrl: './brew-edit.component.css'
})
export class BrewEditComponent {
  isButtonDisabled: boolean = false;
  itemId: number = 0;
  breweries: BreweryClass[] = [];
  beer: BeerClass = {
    id: 0,
    name: '',
    flavour: '',
    age: '',
    breweryId: 1,
    breweryPrice: 0,
    brewery: undefined
  };

  onSubmit() {

    this.isButtonDisabled = true;

    if (this.beer.breweryPrice <= 0) {
      this._snackBar.open('You cant add a beer without price!');
      this.isButtonDisabled = false;
      return;
    }

    if (this.beer.id != 0) {
      delete this.beer.brewery;
      this.breweryPostService.updateItem(this.beer, 'Beers');
    } else {
      this.breweryPostService.postItem(this.beer, 'Beers');
    }

    // Making sure the update is registered before going back to the table
    setTimeout(() => 
    {
      this.route.navigate([`Beers`]);
    },
      1000);
  }

  constructor(private breweryInfoService: BreweryInfoService, private breweryPostService: BreweryPostService, private route: Router, private _snackBar: MatSnackBar) {
    this.breweryInfoService.getInfo('Breweries').subscribe((result) => {
      this.breweries = result;
    });

    if (this.route.getCurrentNavigation()?.extras?.state != undefined){
      this.beer = this.route.getCurrentNavigation()?.extras?.state!['beer'];
    }
  } 
}
