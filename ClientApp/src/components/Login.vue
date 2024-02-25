<template>
    <div class="flex align-items-center w-100 h-100">
        <v-card
            class="mx-auto pa-12 pb-8"
            elevation="8"
            min-width="400"
            rounded="lg"
        >
            <div class="text-subtitle-1 text-medium-emphasis">Аккаунт</div>

            <v-text-field
                v-model.trim="email.value"
                density="compact"
                placeholder="Почта"
                prepend-inner-icon="mdi-email-outline"
                variant="outlined"
                :error-messages="email.errorMessage"
            />

            <div class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between">Пароль</div>

            <v-text-field
                v-model.trim="password.value"
                density="compact"
                placeholder="Введите ваш пароль"
                prepend-inner-icon="mdi-lock-outline"
                variant="outlined"
                :append-inner-icon="password.visible ? 'mdi-eye-off' : 'mdi-eye'"
                :type="password.visible ? 'text' : 'password'"
                :error-messages="password.errorMessage"
                @click:append-inner="password.visible = !password.visible"
            />

            <v-btn
                block
                class="mb-8 login-btn"
                color="blue"
                size="large"
                variant="tonal"
                :loading="loading"
                @click="login"
            >
                Войти
            </v-btn>

            <v-card-text class="text-center">
                <span
                    class="text-blue text-decoration-none register-btn"
                    @click="redirectRegister"
                >
                    Зарегистрироваться <v-icon icon="mdi-chevron-right"></v-icon>
                </span>
            </v-card-text>
        </v-card>
    </div>
</template>

<script lang="ts">
    import {defineComponent} from 'vue'
    import axios, {AxiosError} from "axios";
    import type {User} from "@/interfaces/User";
    import {da} from "vuetify/locale";

    export default defineComponent({
        name: "Auth",

        data() {
            return {
                loading: false,
                email: {
                    value: "",
                    errorMessage: ""
                },
                password: {
                    value: "",
                    errorMessage: "",
                    visible: false
                }
            }
        },

        methods: {
            refreshErrorMessages() {
                this.email.errorMessage = "";
                this.password.errorMessage = "";
            },

            redirectRegister() {
                this.$store.commit("updateShowSearchInput", false);
                this.$router.push("/register");
            },

            validate(): boolean {
                if (this.email.value.length == 0) {
                    this.email.errorMessage = "Заполните email"
                    return false;
                }
                else if (this.password.value.length == 0) {
                    this.password.errorMessage = "Введите пароль"
                    return false;
                }
                return true;
            },

            async login() {
                this.refreshErrorMessages();
                if (this.validate()) {
                    try {
                        this.loading = true;
                        const requestData = {
                            email: this.email.value,
                            password: this.password.value
                        }
                        const response = await axios.post<User>("/api/auth/login", requestData);
                        const data = response.data;
                        this.$cookies.set("user", data.firstName + " " + data.lastName);
                        this.$cookies.set("role", data.roleName);
                        this.$store.commit("updateUser", data.firstName + " " + data.lastName);
                        this.$store.commit("updateRole", data.roleName);
                        this.$router.push("/");
                    }
                    catch (err) {
                        const error = err as AxiosError<any>;
                        if (error.response!.status == 400) {
                            const data = error.response!.data;
                            if (typeof data == "string") {
                                this.email.errorMessage = data;
                            }
                            else {
                                if (data.errors["Email"]) {
                                    this.email.errorMessage = "Некорректный email"
                                }
                                if (data.errors["Password"]) {
                                    this.password.errorMessage = "Минимальная длина пароля 5 символов"
                                }
                            }
                        }
                    }
                    finally {
                        this.loading = false;
                    }
                }
            }
        },

        created() {
            this.$store.commit("updateSearchInput", "");
            this.$store.commit("updateDisableSearchInput", true);
        },
    })
</script>

<style scoped>
    .login-btn {
        margin-top: 10px;
    }

    .register-btn {
        cursor: pointer;
    }
</style>