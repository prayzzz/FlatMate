namespace FlatMate.Account {
    /**
     * Singleton
     * new ApiV1Client() returns the singleton instance
     */
    export class UserService {
        private static instance: UserService;
        private currentUser: User;

        /**
         * Returns the singleton instance
         */
        constructor() {
            if (!UserService.instance) {
                const dataElement = document.getElementById("userData");
                if (dataElement !== null) {
                    this.currentUser = JSON.parse(dataElement.innerHTML);
                }
                else {
                    this.currentUser = { id: 0, userName: 'undefined' };
                }

                UserService.instance = this;
            }

            return UserService.instance;
        }

        public get CurrentUser(): User {
            return this.currentUser;
        }
    }
}