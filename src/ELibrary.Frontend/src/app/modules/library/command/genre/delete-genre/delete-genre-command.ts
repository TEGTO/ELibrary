import { Command, Genre } from "../../../../shared";

export interface DeleteGenreCommand extends Command {
    genre: Genre;
}