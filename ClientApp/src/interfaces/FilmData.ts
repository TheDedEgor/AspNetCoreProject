import type {UserComment} from "@/interfaces/UserComment";

export interface FilmData {
    id: number
    title: string
    genre: string
    description: string
    imageUrl: string
    comments: UserComment[]
}