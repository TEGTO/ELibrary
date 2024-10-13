import { Command, Genre } from "../../../../shared";

export interface UpdateGenreCommand extends Command {
    genre: Genre;
}