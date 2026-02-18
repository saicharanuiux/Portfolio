# Portfolio Project

A full-stack personal portfolio web application built with **React** (frontend) and **ASP.NET Core Web API** (backend).  
It includes authentication (email/password + Google OAuth), resume/profile file access, and a contact form with email notifications.

## Features

- Clean portfolio pages: Home, About, Contact
- Admin-protected routes
- JWT-based authentication
- Google OAuth login flow
- Resume and profile file access (local or Google Drive)
- Contact form with confirmation email workflow
- PostgreSQL + Entity Framework Core persistence
- Redis-based token caching for Google Drive access tokens

## Tech Stack

### Frontend
- React 18
- React Router
- React PDF
- React Icons
- Create React App

### Backend
- ASP.NET Core (.NET 9)
- Entity Framework Core + Npgsql
- JWT Bearer authentication
- Google OAuth
- StackExchange.Redis
- SMTP email service


