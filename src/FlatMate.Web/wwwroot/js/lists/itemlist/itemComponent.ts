namespace flatMate.lists.itemLists {

    export class ItemComponent extends Vue {
        public name = 'item';
        public template = '#item-template';
        public item: Item;
        public props = ['item'];

        constructor(options?: vuejs.ComponentOption) {
            super(options);
        }
    }
}