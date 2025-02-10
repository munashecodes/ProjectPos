import { EntityDto } from "../entity-dtos/entity-dto";
import { Category } from "../enums/category";
import { ProductDto } from "./product-dto";

export interface SubCategoryDto extends EntityDto<number> {
    name?: string;
    description?: string;
    category?: Category;
    products?: ProductDto[];
}
