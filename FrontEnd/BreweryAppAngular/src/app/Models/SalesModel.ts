export class SalesClass {
    public id: number;
    public breweryId: number;
    public wholesalerId: number;
    public beerId: number;
    public quantity: number;
    public saleDate: Date;

    constructor(id: number, breweryId: number, wholesaerId:number, beerId: number, quantity:number, saleDate:Date) {
        this.id = id;
        this.breweryId = breweryId;
        this.beerId = beerId;
        this.wholesalerId = wholesaerId;
        this.quantity = quantity;
        this.saleDate = saleDate;
    }
}
