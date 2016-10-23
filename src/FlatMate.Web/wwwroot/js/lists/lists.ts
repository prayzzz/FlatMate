namespace FlatMate.Lists {
    export function init() {
        Vue.component("item-list-editor", new FlatMate.Lists.ItemLists.ItemListEditor());
        Vue.component("item-list-group", new FlatMate.Lists.ItemLists.ItemListGroupComponent());
        Vue.component("item-list-item", new FlatMate.Lists.ItemLists.ItemListItemComponent());
    }    
}