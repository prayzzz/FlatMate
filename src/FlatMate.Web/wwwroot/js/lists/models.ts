﻿namespace FlatMate.Lists {
    export interface ItemList {
        creationDate: Date;
        id: number;
        isPublic: boolean;
        items: Item[];
        listGroups: ItemListGroup[];
        lastModified: Date;
        name: string;
        userId: number;
        user: Account.User;
    }

    export interface ItemListGroup {
        creationDate?: Date;
        id?: number;
        items?: Item[];
        itemListId?: number;
        lastModified?: Date;
        name: string;
        userId?: number;
        user?: Account.User;
    }

    export interface Item {
        creationDate?: Date;
        id?: number;
        itemListId?: number;
        itemListGroupId?: number;
        lastModified?: Date;
        userId?: number;
        user?: Account.User;
        value: string;
    }
}