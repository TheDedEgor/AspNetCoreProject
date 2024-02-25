<template>
    <v-main class="flex align-items-center">
        <v-window
            v-model="window"
            class="window"
            show-arrows
        >
            <v-window-item
                class="window-item"
                v-for="(page, idx1) in pages"
                :key="idx1"
            >
                <v-card
                    class="card-item"
                    variant="outlined"
                    v-for="(film, idx2) in page"
                    :key="idx2"
                    :title="film.title"
                    :subtitle="film.genre"
                    @click="openFilm(film.id)"
                >
                    <v-img
                        height="500"
                        class="film-image"
                        :src="film.imageUrl"
                        cover
                    />
                </v-card>
            </v-window-item>
        </v-window>
    </v-main>
</template>

<script lang="ts">
    import {defineComponent} from 'vue'
    import axios from "axios";
    import type {FilmShortData} from "@/interfaces/FilmShortData";

    interface Data {
        window: number
        pages: FilmShortData[][]
        films: FilmShortData[]
    }

    export default defineComponent({
        name: "index",

        data(): Data {
            return {
                window: 0,
                pages: [],
                films: []
            }
        },

        watch: {
            "$store.state.searchInput"(val) {
                if (val == "") {
                    this.getAllFilms();
                }
                else {
                    this.searchFilms(val);
                }
            }
        },

        methods: {
            openFilm(id: number) {
                this.$store.commit("updateLastWindow", this.window);
                this.$router.push(`/film/${id}`);
            },

            async getAllFilms() {
                const response = await axios.get<FilmShortData[]>("/api/films");
                this.films = response.data;
                this.fillWindows();
            },

            async searchFilms(text: string) {
                const response = await axios.get<FilmShortData[]>(`/api/films/search?text=${text}`);
                this.films = response.data;
                this.fillWindows();
            },

            fillWindows() {
                this.pages = [];
                const size = 4;
                for (let i = 0; i < Math.ceil(this.films.length / size); i++) {
                    this.pages.push(this.films.slice(i * size, (i * size) + size));
                }
            }
        },

        async created() {
            this.$store.commit("updateDisableSearchInput", false);
            this.window = this.$store.state.lastWindow;
            await this.getAllFilms();
        }
    })
</script>

<style scoped>
    .window {
        width: 100%;
        height: 100%;
        margin: 0 15px;
    }

    .card-item {
        cursor: pointer;
        width: 20%;
    }

    .card-item:hover {
        animation: 1.2s ease-in-out 0s normal none infinite running trambling-animation;
    }

    .window-item {
        display: flex;
        justify-content: space-evenly;
        align-items: center;
        height: 100%;
    }

    .film-image {
        margin: 0 25px 25px 25px;
    }

    @keyframes trambling-animation {
        0%, 50%, 100% {
            transform: rotate(0deg);
        }
        10%, 30% {
            transform: rotate(-3deg);
        }
        20%, 40% {
            transform: rotate(3deg);
        }
    }
</style>