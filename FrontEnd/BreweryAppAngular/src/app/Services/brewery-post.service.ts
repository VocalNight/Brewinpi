import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BreweryPostService {

  private apiUrl = "https://localhost:7036/api"

  constructor(private http: HttpClient) { }

  postItem(item: any, model: string) {
    this.http.post(`${this.apiUrl}/${model}`, item)
      .subscribe({
        next: (r) => console.log("Api sucess", r),
        error: (e) => console.error("Api error", e)
      })
  }

  updateItem(item: any, model: string) {

    this.http.put(`${this.apiUrl}/${model}/${item.id}`, item)
      .subscribe({
        next: (r) => console.log("Api sucess", r),
        error: (e) => console.error("Api error", e)
      })
  }

  deleteRow(id: number, model: string) {

    this.http.delete(`${this.apiUrl}/${model}/${id}`)
      .subscribe({
        next: (r) => {
          console.log("Api sucess", r);
        },
        error: (e) => console.error("Api error", e)
      })
  }
}
