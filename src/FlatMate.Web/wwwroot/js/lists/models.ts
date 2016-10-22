namespace flatMate.lists {
    export interface ItemList {
        creationDate: Date;
        id: number;
        isPublic: boolean;
        items: Item[];
        listGroups: ItemListGroup[];
        lastModified: Date;
        name: string;
        userId: number;
    }

    export interface ItemListGroup {
        creationDate: Date;
        id: number;
        items: Item[];
        lastModified: Date;
        name: string;
        userId: number;
    }

    export interface Item {
        creationDate: Date;
        id: number;
        lastModified: Date;
        userId: number;
        value: string;
    }
}