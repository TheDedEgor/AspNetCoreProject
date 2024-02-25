import {Store} from "vuex";

declare module '@vue/runtime-core' {
    interface State {
        user: string | null
        role: string | null
        showServerErrorModal: boolean
        lastWindow: number
        searchInput: string
        disableSearchInput: boolean
    }

    interface ComponentCustomProperties {
        $store: Store<State>
    }
}
