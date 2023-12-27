import { BeerClass } from "./BeersModel";
import { SalesClass } from "./SalesModel";
import { WholesalerStockClass } from "./WholesalerStockModel";

export class WholesalerClass {
    public id: number;
    public name: string;
    public stockLimit: number;
    public sales?: SalesClass[];
    public stocks?: WholesalerStockClass[];
    public allowedBeers?: BeerClass[];

    constructor(id: number, name: string, stockLimit: number, sales: SalesClass[], stocks: WholesalerStockClass[], allowedBeers: BeerClass[]) {
        this.id = id;
        this.stockLimit = stockLimit;
        this.name = name;
        this.sales = sales;
        this.stocks = stocks;
        this.allowedBeers = allowedBeers;
    }
}
