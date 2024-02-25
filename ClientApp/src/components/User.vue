<template>
    <v-main class="user-block">
        <v-card
            class="user-card"
            title="Данные о пользователе"
            variant="outlined"
        >
            <div class="user-data-block">
                <div class="user-data">
                    <v-text-field
                        label="Почта"
                        hide-details
                        readonly
                        :model-value="userData?.email"
                    ></v-text-field>
                    <v-text-field
                        label="Имя"
                        hide-details
                        readonly
                        :model-value="userData?.firstName"
                    ></v-text-field>

                    <v-text-field
                        label="Фамилия"
                        hide-details
                        readonly
                        :model-value="userData?.lastName"
                    ></v-text-field>
                </div>

                <v-card-actions>
                    <v-btn
                        elevation="2"
                        class="logout-btn"
                        append-icon="mdi-logout"
                        @click="logout"
                    >
                        Выйти
                    </v-btn>
                </v-card-actions>
            </div>
        </v-card>

        <v-card
            v-if="$store.state.role == 'admin'"
            class="card-item"
            title="Администрирование комментариев"
            variant="outlined"
        >
            <div class="table-block">
                <v-data-table
                    :loading="loading"
                    height="400"
                    no-data-text="Комментриаев пока нет"
                    items-per-page-text="Элементов на странице"
                    :headers="commentTable.headers"
                    :items="commentTable.data"
                >
                    <template v-slot:item.actions="{item}">
                        <v-icon
                            icon="mdi-delete"
                            @click="deleteComment((item as Comment).id)"
                        />
                    </template>
                </v-data-table>
            </div>
        </v-card>

        <v-dialog
            v-model="dialog"
            width="auto"
            @close="closeModal"
        >
            <v-card>
                <v-card-title class="text-h5">
                    Внимание
                </v-card-title>
                <v-card-text>Вы действительно хотите удалить комментарий?</v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn
                        color="green-darken-1"
                        variant="text"
                        @click="closeModal"
                    >
                        Отмена
                    </v-btn>
                    <v-btn
                        color="green-darken-1"
                        variant="text"
                        @click="confirmDelete"
                    >
                        Подтвердить
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </v-main>
</template>

<script lang="ts">
    import {defineComponent} from 'vue'
    import axios from "axios";
    import type {UserData} from "@/interfaces/User";
    import type {Comment} from "@/interfaces/Comment";

    interface Data {
        dialog: boolean
        loading: boolean
        commentTable: any
        userData: UserData | null
        commentId: number | null
    }

    export default defineComponent({
        name: "User",

        data(): Data {
            return  {
                dialog: false,
                loading: false,
                userData: null,
                commentId: null,
                commentTable: {
                    headers: [
                        { key: 'filmName', title: 'Фильм' },
                        { key: 'userName', title: 'Пользователь' },
                        { key: 'comment', title: 'Комментарий' },
                        { key: 'actions', title: 'Действия', width: "25px" },
                    ],
                    data: [] as Comment[]
                }
            }
        },

        methods: {
            async getUserData() {
                const response = await axios.get("/api/users");
                return response.data;
            },

            async logout() {
                await axios.post("/api/auth/logout");
                this.$cookies.remove("user");
                this.$cookies.remove("role");
                this.$store.commit("updateUser", null);
                this.$store.commit("updateRole", null);
                await this.$router.push("/");
            },

            async getAllComments() {
                try {
                    this.loading = true;
                    const response = await axios.get<Comment[]>("/api/comments");
                    return response.data;
                }
                finally {
                    this.loading = false;
                }
            },

            closeModal() {
                this.commentId = null;
                this.dialog = false;
            },

            deleteComment(id: number) {
                this.commentId = id;
                this.dialog = true;
            },

            async confirmDelete() {
                await axios.delete(`/api/comments?id=${this.commentId}`);
                this.commentTable.data = await this.getAllComments();
                this.closeModal();
            }
        },

        async created() {
            this.$store.commit("updateSearchInput", "");
            this.$store.commit("updateDisableSearchInput", true);
            this.userData = await this.getUserData();
            if (this.$store.state.role == 'admin') {
                this.commentTable.data = await this.getAllComments();
            }
        }
    })
</script>

<style scoped>
    .user-card {
        height: max-content;
        padding-bottom: 10px;
    }

    .user-block {
        display: flex;
        flex-direction: column;
        gap: 50px;
        margin: 40px;
    }

    .logout-btn {
        margin-left: auto;
        margin-right: 15px;
    }

    .user-data-block {
        display: flex;
        flex-direction: column;
    }

    .user-data {
        display: flex;
        padding: 20px;
        gap: 20px;
    }

    .table-block {
        padding: 0 20px;
    }
</style>