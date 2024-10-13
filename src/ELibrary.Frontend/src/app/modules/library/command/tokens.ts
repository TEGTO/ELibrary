import { InjectionToken } from "@angular/core";
import { CreateAuthorCommand, CreateBookCommand, CreateGenreCommand, CreatePublisherCommand, DeleteAuthorCommand, DeleteBookCommand, DeleteGenreCommand, DeletePublisherCommand, UpdateAuthorCommand, UpdateBookCommand, UpdateGenreCommand, UpdatePublisherCommand } from "..";
import { CommandHandler } from "../../shared";

export const CREATE_AUTHOR_COMMAND_HANDLER = new InjectionToken<CommandHandler<CreateAuthorCommand>>('CreateAuthorCommandHandler');
export const UPDATE_AUTHOR_COMMAND_HANDLER = new InjectionToken<CommandHandler<UpdateAuthorCommand>>('UpdateAuthorCommandHandler');
export const DELETE_AUTHOR_COMMAND_HANDLER = new InjectionToken<CommandHandler<DeleteAuthorCommand>>('DeleteAuthorCommandHandler');

export const CREATE_GENRE_COMMAND_HANDLER = new InjectionToken<CommandHandler<CreateGenreCommand>>('CreateGenreCommandHandler');
export const UPDATE_GENRE_COMMAND_HANDLER = new InjectionToken<CommandHandler<UpdateGenreCommand>>('UpdateGenreCommandHandler');
export const DELETE_GENRE_COMMAND_HANDLER = new InjectionToken<CommandHandler<DeleteGenreCommand>>('DeleteGenreCommandHandler');

export const CREATE_PUBLISHER_COMMAND_HANDLER = new InjectionToken<CommandHandler<CreatePublisherCommand>>('CreatePublisherCommandHandler');
export const UPDATE_PUBLISHER_COMMAND_HANDLER = new InjectionToken<CommandHandler<UpdatePublisherCommand>>('UpdatePublisherCommandHandler');
export const DELETE_PUBLISHER_COMMAND_HANDLER = new InjectionToken<CommandHandler<DeletePublisherCommand>>('DeletePublisherCommandHandler');

export const CREATE_BOOK_COMMAND_HANDLER = new InjectionToken<CommandHandler<CreateBookCommand>>('CreateBookCommandHandler');
export const UPDATE_BOOK_COMMAND_HANDLER = new InjectionToken<CommandHandler<UpdateBookCommand>>('UpdateBookCommandHandler');
export const DELETE_BOOK_COMMAND_HANDLER = new InjectionToken<CommandHandler<DeleteBookCommand>>('DeleteBookCommandHandler');