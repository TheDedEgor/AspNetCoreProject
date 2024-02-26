<template>
    <v-card class="h-100">
        <v-layout class="h-100">
            <v-app-bar
                color="primary"
                prominent
            >
                <v-app-bar-nav-icon
                    icon="mdi-comment"
                    @click="redirectHome"
                />

                <v-toolbar-title>Отзовик фильмов</v-toolbar-title>

                <v-spacer/>

                <v-text-field
                    v-model="$store.state.searchInput"
                    class="search-input"
                    density="compact"
                    variant="solo"
                    label="Поиск фильмов"
                    append-inner-icon="mdi-magnify"
                    single-line
                    hide-details
                    :disabled="$store.state.disableSearchInput"
                    @update:modelValue="updateSearchInput"
                />

                <v-btn
                    v-if="$store.state.user"
                    prepend-icon="mdi-account"
                    @click="openPersonalAccount"
                >
                    {{ $store.state.user }}
                </v-btn>
                <v-btn
                    v-else
                    icon="mdi-login"
                    @click="redirectLogin"
                />
            </v-app-bar>

            <RouterView/>
        </v-layout>
    </v-card>

    <v-dialog
        width="auto"
        :model-value="$store.state.showServerErrorModal"
    >
        <v-card>
            <v-card-text>
                Произошла ошибка на сервере
            </v-card-text>
            <v-card-actions>
                <v-btn
                    color="primary"
                    block
                    @click="closeModal"
                >
                    Закрыть
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script lang="ts">
    import {defineComponent} from "vue";
    import axios, {type AxiosError} from "axios";

    export default defineComponent({
        methods: {
            redirectHome() {
                this.$router.push("/");
            },

            closeModal() {
                this.$store.commit("updateShowServerErrorModal", false);
            },

            redirectLogin() {
                this.$router.push("/login");
            },

            openPersonalAccount() {
                this.$router.push("/user");
            },

            updateSearchInput(text: string) {
                this.$store.commit("updateSearchInput", text);
            }
        },

        created() {
            this.$store.commit("updateUser", this.$cookies.get("user"));
            this.$store.commit("updateRole", this.$cookies.get("role"));
            axios.defaults.withCredentials = true;
            axios.defaults.baseURL = import.meta.env.VITE_BASE_URL;
            axios.interceptors.response.use(null, (err) => {
                const error = err as AxiosError<any>;
                if (error.response!.status >= 500) {
                    this.$store.commit("updateShowServerErrorModal", true);
                }
                else if (error.response!.status == 401) {
                    this.$cookies.remove("user");
                    this.$cookies.remove("role");
                    this.$store.commit("updateUser", null);
                    this.$store.commit("updateRole", null);
                    this.$router.push("/login");
                }
                else {
                    throw err;
                }
            });
        }
    })
</script>

<style scoped>
    .search-input {
        max-width: 300px;
        margin-right: 15px;
    }
</style>
