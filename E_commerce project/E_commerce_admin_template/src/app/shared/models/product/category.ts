export interface Category {
    id: number,
    categoryName: string,
    motherCategoryId: number,
    imageUrl: string,
    description: string,
    isActive: boolean
}