<template>
    <v-main class="film-block">
        <div class="film-description w-50">
            <v-card
                height="500"
                width="35%"
                :image="filmData?.imageUrl"
                :title="filmData?.title"
                theme="dark"
            />
            <v-card
                class="card-item"
                title="Описание"
                variant="outlined"
                width="65%"
                height="500"
            >
                <v-card-text>
                    {{ filmData?.description }}
                </v-card-text>
            </v-card>
        </div>

        <div class="comments-block">
            <v-list
                class="comments"
                height="500"
                max-height="500"
                elevation="3"
                :lines="false"
            >
                <v-list-subheader>Комментарии</v-list-subheader>

                <v-list-item
                    v-if=" filmData!.comments.length > 0"
                    v-for="comment in filmData?.comments"
                    :key="comment.id"
                    :title="comment.fullName"
                    :subtitle="comment.comment"
                />
                <v-card-text
                    v-else
                    class="no-comment"
                >
                    Комментариев пока нет
                </v-card-text>
            </v-list>
            <v-textarea
                v-model.trim="comment.value"
                class="comment-textarea"
                label="Комментарий"
                append-icon="mdi-send"
                max-rows="5"
                variant="outlined"
                shaped
                auto-grow
                clearable
                no-resize
                :loading="loading"
                :error-messages="comment.errorMessage"
                @keydown="keydownTextarea"
                @click:append="addComment"
            ></v-textarea>
        </div>

    </v-main>
</template>

<script lang="ts">
    import {defineComponent} from 'vue'
    import axios from "axios";
    import type {FilmData} from "@/interfaces/FilmData";
    import {ca} from "vuetify/locale";

    interface Data {
        loading: boolean
        comment: {
            value: string,
            errorMessage: string
        }
        filmData: FilmData
    }

    export default defineComponent({
        name: "Film",

        data(): Data {
            return {
                loading: false,
                comment: {
                    value: "",
                    errorMessage: ""
                },
                filmData: {
                    id: -1,
                    genre: "",
                    title: "",
                    description: "",
                    imageUrl: "",
                    comments: []
                }
            }
        },

        methods: {
            async getFilmData(id: number) {
                const response = await axios.get<FilmData>(`/api/films/${id}`);
                return response.data;
            },

            async addComment() {
                this.comment.errorMessage = "";
                if (this.comment.value.length > 0) {
                    try {
                        this.loading = true;
                        const data = {
                            filmId: this.filmData?.id,
                            comment: this.comment.value
                        }
                        await axios.post("/api/comments", data);
                        const id = Number(this.$route.params.id);
                        this.filmData = await this.getFilmData(id);
                    }
                    finally {
                        this.comment.value = "";
                        this.loading = false;
                    }
                }
                else {
                    this.comment.errorMessage = "Введите комментарий";
                }
            },

            keydownTextarea(event: KeyboardEvent) {
                if (!event.shiftKey && event.code == "Enter") {
                    event.preventDefault();
                    this.addComment();
                }
            }
        },

        async created() {
            this.$store.commit("updateSearchInput", "");
            this.$store.commit("updateDisableSearchInput", true);
            const id = Number(this.$route.params.id);
            this.filmData = await this.getFilmData(id);
        }
    })
</script>

<style scoped>
    .film-description {
        display: flex;
        align-items: center;
        gap: 50px;
    }

    .film-block {
        display: flex;
        justify-content: space-around;
        margin: 45px 0;
    }

    .comments-block {
        display: flex;
        flex-direction: column;
        gap: 30px;
        width: 40%;
    }

    .comments::-webkit-scrollbar {
        width: 5px;
    }

    .comments::-webkit-scrollbar-thumb {
        background-color: #6252EF;
        border-radius: 10px;
    }

    .no-comment {
        color: #D8D8D8;
        font-size: 20px;
        display: flex;
        align-items: center;
        justify-content: center;
        height: calc(100% - 100px);
    }
</style>