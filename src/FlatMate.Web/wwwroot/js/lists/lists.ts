namespace flatMate.lists {
    export function init() {
        Vue.component("item-list-editor", new flatMate.lists.itemLists.ItemListEditor());
        Vue.component("item-list-group", new flatMate.lists.itemLists.ItemListGroupComponent());
        //Vue.component("item", new flatMate.lists.itemLists.ItemComponent());
    }    
}