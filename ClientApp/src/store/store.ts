import {createStore} from "vuex";

const store= createStore({
    state: {
        user: null,
        role: null,
        showServerErrorModal: false,
        lastWindow: 0,
        searchInput: "",
        disableSearchInput: false
    },

    mutations: {
        updateShowServerErrorModal (state: any, payload: boolean) {
            state.showServerErrorModal = payload;
        },
        updateUser (state: any, payload: string | null) {
            state.user = payload;
        },
        updateRole (state: any, payload: string | null) {
            state.role = payload;
        },
        updateLastWindow (state: any, payload: number) {
            state.lastWindow = payload;
        },
        updateSearchInput (state: any, payload: string) {
            state.searchInput = payload;
        },
        updateDisableSearchInput(state: any, payload: boolean) {
            state.disableSearchInput = payload;
        }
    }
})

export default store;