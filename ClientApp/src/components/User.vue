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
            variant="outlined"
            height="470"
        >
            <v-card-title class="films-header">
                Администрирование фильмов
                <v-btn
                    color="indigo-darken-3"
                    append-icon="mdi-plus"
                    @click="addFilm"
                >
                    Добавить
                </v-btn>
            </v-card-title>

            <div class="table-block">
                <v-data-table
                    :loading="filmLoading"
                    height="320"
                    no-data-text="Фильмов пока нет"
                    items-per-page-text="Элементов на странице"
                    :headers="filmTable.headers"
                    :items="filmTable.data"
                    :items-per-page="5"
                    :items-per-page-options="[5]"
                >
                    <template v-slot:item.imageUrl="{item}">
                        <a :href="(item as FilmData).imageUrl" target="_blank">
                            {{ (item as FilmData).imageUrl }}
                        </a>
                    </template>

                    <template v-slot:item.actions="{item}">
                        <v-icon
                            icon="mdi-pencil"
                            @click="updateFilm((item as FilmData))"
                        />
                        <v-icon
                            icon="mdi-delete"
                            @click="deleteFilm((item as FilmData).id)"
                        />
                    </template>
                </v-data-table>
            </div>
        </v-card>

        <v-card
            v-if="$store.state.role == 'admin'"
            title="Администрирование комментариев"
            variant="outlined"
            height="470"
        >
            <div class="table-block">
                <v-data-table
                    :loading="commentLoading"
                    height="320"
                    no-data-text="Комментриаев пока нет"
                    items-per-page-text="Элементов на странице"
                    :headers="commentTable.headers"
                    :items="commentTable.data"
                    :items-per-page="5"
                    :items-per-page-options="[5]"
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
            v-model="deleteCommentDialog"
            width="auto"
            @close="closeDeleteCommentModal"
        >
            <v-card class="delete-comment-modal">
                <v-card-title class="text-h5">
                    Внимание
                </v-card-title>
                <v-card-text>Вы действительно хотите удалить комментарий?</v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn
                        color="green-darken-1"
                        variant="text"
                        @click="closeDeleteCommentModal"
                    >
                        Отмена
                    </v-btn>
                    <v-btn
                        color="green-darken-1"
                        variant="text"
                        @click="confirmDeleteComment"
                    >
                        Подтвердить
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>

        <v-dialog
            v-model="deleteFilmDialog"
            width="auto"
            @close="closeDeleteFilmModal"
        >
            <v-card class="delete-comment-modal">
                <v-card-title class="text-h5">
                    Внимание
                </v-card-title>
                <v-card-text>Вы действительно хотите удалить фильм и все комментарии к нему?</v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn
                        color="green-darken-1"
                        variant="text"
                        @click="closeDeleteFilmModal"
                    >
                        Отмена
                    </v-btn>
                    <v-btn
                        color="green-darken-1"
                        variant="text"
                        @click="confirmDeleteFilm"
                    >
                        Подтвердить
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>

        <v-dialog
            v-model="updateFilmDialog.show"
            width="650"
            @close="closeUpdateFilmModal"
        >
            <v-card
                class="update-film-modal"
                :title="updateFilmDialog.modalTitle"
            >
                <v-card-text>
                    <v-text-field
                        v-model.trim="updateFilmDialog.title"
                        label="Название"
                    />
                    <v-text-field
                        v-model.trim="updateFilmDialog.genre"
                        label="Жанр"
                    />
                    <v-text-field
                        v-model.trim="updateFilmDialog.imageUrl"
                        label="URL-картинки"
                    />
                    <v-textarea
                        v-model.trim="updateFilmDialog.description"
                        label="Описание"
                        no-resize
                    />
                </v-card-text>

                <v-card-actions>
                    <v-spacer></v-spacer>

                    <v-btn
                        text="Отмена"
                        variant="plain"
                        @click="closeUpdateFilmModal"
                    ></v-btn>

                    <v-btn
                        color="primary"
                        variant="tonal"
                        :text="updateFilmDialog.confirmBtnText"
                        :disabled="disableUpdateFilmBtn"
                        @click="confirmUpdateFilm"
                    ></v-btn>
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
    import type {FilmShortData} from "@/interfaces/FilmShortData";
    import type {FilmData} from "@/interfaces/FilmData";

    interface Data {
        tab: number
        commentLoading: boolean
        filmLoading: boolean
        filmTable: any
        commentTable: any
        userData: UserData | null
        filmId: number | null
        commentId: number | null
        updateFilmDialog: {
            id: number
            show: boolean
            title: string
            genre: string
            imageUrl: string
            description: string
            modalTitle: string
            confirmBtnText: string
            isEdit: boolean
        }
        deleteCommentDialog: boolean
        deleteFilmDialog: boolean
    }

    export default defineComponent({
        name: "User",

        data(): Data {
            return {
                tab: 0,
                deleteFilmDialog: false,
                deleteCommentDialog: false,
                updateFilmDialog: {
                    id: -1,
                    show: false,
                    title: "",
                    genre: "",
                    imageUrl: "",
                    description: "",
                    modalTitle: "Добавление фильма",
                    confirmBtnText: "Добавить",
                    isEdit: false
                },
                commentLoading: false,
                filmLoading: false,
                userData: null,
                filmId: null,
                commentId: null,
                commentTable: {
                    headers: [
                        {key: 'filmName', title: 'Фильм'},
                        {key: 'userName', title: 'Пользователь'},
                        {key: 'comment', title: 'Комментарий'},
                        {key: 'actions', title: 'Действия', width: "25px"},
                    ],
                    data: [] as Comment[]
                },
                filmTable: {
                    headers: [
                        {key: 'title', title: 'Название'},
                        {key: 'genre', title: 'Жанр'},
                        {key: 'imageUrl', title: 'URL картинки'},
                        {key: 'actions', title: 'Действия', width: "25px"}
                    ],
                    data: [] as FilmData[]
                }
            }
        },

        computed: {
            disableUpdateFilmBtn(): boolean {
                return !(this.updateFilmDialog.title && this.updateFilmDialog.genre &&
                    this.updateFilmDialog.imageUrl && this.updateFilmDialog.description);
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

            async getAllFilms() {
                try {
                    this.filmLoading = true;
                    const response = await axios.get<FilmData[]>("/api/admin/films");
                    return response.data;
                } finally {
                    this.filmLoading = false;
                }
            },

            async getAllComments() {
                try {
                    this.commentLoading = true;
                    const response = await axios.get<Comment[]>("/api/admin/comments");
                    return response.data;
                } finally {
                    this.commentLoading = false;
                }
            },

            closeDeleteCommentModal() {
                this.commentId = null;
                this.deleteCommentDialog = false;
            },

            closeUpdateFilmModal() {
                this.updateFilmDialog.show = false;
            },

            deleteComment(id: number) {
                this.commentId = id;
                this.deleteCommentDialog = true;
            },

            async confirmDeleteComment() {
                await axios.delete(`/api/admin/comments?id=${this.commentId}`);
                this.commentTable.data = await this.getAllComments();
                this.closeDeleteCommentModal();
            },

            addFilm() {
                this.updateFilmDialog.confirmBtnText = "Добавить";
                this.updateFilmDialog.modalTitle = "Добавление фильма";
                this.updateFilmDialog.id = -1;
                this.updateFilmDialog.title = "";
                this.updateFilmDialog.genre = "";
                this.updateFilmDialog.imageUrl = "";
                this.updateFilmDialog.description = "";
                this.updateFilmDialog.isEdit = false;
                this.updateFilmDialog.show = true;
            },

            updateFilm(film: FilmData) {
                this.updateFilmDialog.confirmBtnText = "Изменить";
                this.updateFilmDialog.modalTitle = "Изменение фильма";
                this.updateFilmDialog.id = film.id;
                this.updateFilmDialog.title = film.title;
                this.updateFilmDialog.genre = film.genre;
                this.updateFilmDialog.imageUrl = film.imageUrl;
                this.updateFilmDialog.description = film.description;
                this.updateFilmDialog.isEdit = true;
                this.updateFilmDialog.show = true;
            },

            async confirmUpdateFilm() {
                const data: FilmData = {
                    id: this.updateFilmDialog.id,
                    title: this.updateFilmDialog.title,
                    genre: this.updateFilmDialog.genre,
                    imageUrl: this.updateFilmDialog.imageUrl,
                    description: this.updateFilmDialog.description,
                    comments: []
                }
                if (this.updateFilmDialog.isEdit) {
                    await axios.put("/api/admin/films", data);
                }
                else {
                    await axios.post("/api/admin/films", data);
                }
                this.filmTable.data = await this.getAllFilms();
                this.closeUpdateFilmModal();
            },

            deleteFilm(id: number) {
                this.filmId = id;
                this.deleteFilmDialog = true;
            },

            closeDeleteFilmModal() {
                this.filmId = null;
                this.deleteFilmDialog = false;
            },

            async confirmDeleteFilm() {
                await axios.delete(`/api/admin/films?id=${this.filmId}`);
                this.filmTable.data = await this.getAllFilms();
                this.commentTable.data = await this.getAllComments();
                this.closeDeleteFilmModal();
            }
        },

        async created() {
            this.$store.commit("updateSearchInput", "");
            this.$store.commit("updateDisableSearchInput", true);
            this.userData = await this.getUserData();
            if (this.$store.state.role == 'admin') {
                this.commentTable.data = await this.getAllComments();
                this.filmTable.data = await this.getAllFilms();
            }
        }
    })
</script>

<style scoped>
    .update-film-modal {
        padding: 10px 3px;
    }

    .delete-comment-modal {
        padding: 10px 3px;
    }

    .films-header {
        display: flex;
        justify-content: space-between;
    }

    .user-card {
        height: max-content;
        padding-bottom: 10px;
    }

    .user-block::-webkit-scrollbar {
        width: 5px;
    }

    .user-block::-webkit-scrollbar-thumb {
        background-color: #6252EF;
        border-radius: 10px;
    }

    .user-block {
        overflow-y: scroll;
        padding: 100px 40px 30px 40px;
        display: grid;
        gap: 50px;
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