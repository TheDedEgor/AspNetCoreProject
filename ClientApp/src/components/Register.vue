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
                v-model.trim="firstName.value"
                density="compact"
                placeholder="Имя"
                variant="outlined"
                :error-messages="firstName.errorMessage"
            ></v-text-field>

            <v-text-field
                v-model.trim="lastName.value"
                density="compact"
                placeholder="Фамилия"
                variant="outlined"
                :error-messages="lastName.errorMessage"
            ></v-text-field>

            <v-text-field
                v-model.trim="email.value"
                density="compact"
                placeholder="Почта"
                variant="outlined"
                :error-messages="email.errorMessage"
            ></v-text-field>

            <div class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between">Пароль</div>

            <v-text-field
                v-model.trim="password.value"
                :append-inner-icon="password.visible ? 'mdi-eye-off' : 'mdi-eye'"
                :type="password.visible ? 'text' : 'password'"
                :error-messages="password.errorMessage"
                density="compact"
                placeholder="Введите ваш пароль"
                variant="outlined"
                @click:append-inner="password.visible = !password.visible"
            ></v-text-field>

            <v-text-field
                v-model.trim="repeatPassword.value"
                :append-inner-icon="repeatPassword.visible ? 'mdi-eye-off' : 'mdi-eye'"
                :type="repeatPassword.visible ? 'text' : 'password'"
                :error-messages="repeatPassword.errorMessage"
                density="compact"
                placeholder="Повторите пароль"
                variant="outlined"
                @click:append-inner="repeatPassword.visible = !repeatPassword.visible"
            ></v-text-field>

            <v-btn
                block
                class="mb-8"
                color="blue"
                size="large"
                variant="tonal"
                :loading="loading"
                @click="register"
            >
                Регистрация
            </v-btn>
        </v-card>
    </div>
</template>

<script lang="ts">
    import {defineComponent} from 'vue'
    import axios, {AxiosError} from "axios";
    import type {User} from "@/interfaces/User";

    export default defineComponent({
        name: "Register",

        data() {
            return {
                loading: false,
                firstName: {
                    value: "",
                    errorMessage: ""
                },
                lastName: {
                    value: "",
                    errorMessage: ""
                },
                email: {
                    value: "",
                    errorMessage: ""
                },
                password: {
                    value: "",
                    errorMessage: "",
                    visible: false
                },
                repeatPassword: {
                    value: "",
                    errorMessage: "",
                    visible: false
                }
            }
        },

        methods: {
            refreshErrorMessages() {
                this.firstName.errorMessage = "";
                this.lastName.errorMessage = "";
                this.email.errorMessage = "";
                this.password.errorMessage = "";
                this.repeatPassword.errorMessage = "";
            },

            validate(): boolean {
                if (this.firstName.value.length == 0) {
                    this.firstName.errorMessage = "Введите имя";
                    return false;
                }
                else if (this.lastName.value.length == 0) {
                    this.lastName.errorMessage = "Введите фамилию"
                    return false;
                }
                else if (this.email.value.length == 0) {
                    this.email.errorMessage = "Заполните email"
                    return false;
                }
                else if (this.password.value.length == 0) {
                    this.password.errorMessage = "Введите пароль"
                    return false;
                }
                else if (this.repeatPassword.value != this.password.value) {
                    this.repeatPassword.errorMessage = "Пароли не совпадают"
                    return false;
                }
                return true;
            },

            async register() {
                this.refreshErrorMessages();
                if (this.validate()) {
                    try {
                        this.loading = true;
                        const requestData = {
                            firstName: this.firstName.value,
                            lastName: this.lastName.value,
                            email: this.email.value,
                            password: this.password.value
                        }
                        const response = await axios.post<User>("/api/auth/register", requestData);
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
        }
    })
</script>

<style scoped>
</style>