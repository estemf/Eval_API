# Documentation API

## Introduction

Cette documentation couvre l'ensemble des fonctionnalités de l'API, incluant les points d'entrée principaux, les méthodes HTTP disponibles et les formats de données requis pour interagir avec le système.

L'API permet de :
- Gérer les utilisateurs (authentification, profil, etc.).
- Administrer les équipes.
- Gérer les challenges et leurs questions associées.
- Vérifier les réponses aux questions.

---

## Partie 1 : Explicative

### Points importants

1. **Structure RESTful** :
   L'API suit une architecture RESTful avec des ressources accessibles via des URL définies.

2. **Authentification** :
   Les opérations nécessitent un jeton JWT, obtenu après authentification (`/api/Auth/login`).

3. **Gestion des erreurs** :
   - Code 400 : Requête invalide (exemple : champs manquants).
   - Code 404 : Ressource non trouvée.
   - Code 500 : Erreur interne.

4. **Format des données** :
   Toutes les données doivent être envoyées au format JSON.

---

## Partie 2 : Pratique

### Authentification

#### Login
- **URL** : `POST /api/Auth/login`
- **Body** :
  ```json
  {
    "email": "user@example.com",
    "password": "Password123!"
  }
  ```
- **Réponse** : `200 OK` ou `401 Unauthorized`

#### Register
- **URL** : `POST /api/Auth/register`
- **Body** :
  ```json
  {
    "username": "newuser",
    "email": "newuser@example.com",
    "password": "Password123!",
    "passwordConfirmatiion": "Password123!"
  }
  ```
- **Réponse** : `200 OK` ou `400 Bad Request`

#### Logout
- **URL** : `POST /api/Auth/logout`
- **Body** : Vide
- **Réponse** : `200 OK`

---

### Utilisateurs

#### Récupérer tous les utilisateurs
- **URL** : `GET /api/Users`
- **Réponse** : Liste des utilisateurs
  ```json
  [
    {
      "id": "GUID",
      "username": "user1",
      "email": "user1@example.com",
      "role": "Admin",
      "teams": ["Team A"]
    }
  ]
  ```

#### Modifier une image de profil
- **URL** : `PUT /api/Users/ProfilePicture/{id}`
- **Body** :
  ```json
  "https://example.com/profile-picture.jpg"
  ```
- **Réponse** : `204 No Content`

---

### Équipes

#### Créer une équipe
- **URL** : `POST /api/Teams`
- **Body** :
  ```json
  {
    "name": "Team X",
    "userIds": ["GUID1", "GUID2"]
  }
  ```
- **Réponse** : Détails de l'équipe créée

#### Supprimer une équipe
- **URL** : `DELETE /api/Teams/{id}`
- **Réponse** : `200 OK`

---

### Challenges

#### Liste des challenges
- **URL** : `GET /api/ChallengesInfos`
- **Réponse** :
  ```json
  [
    {
      "id": 1,
      "title": "Challenge 1",
      "difficulty": "Easy",
      "points": 10,
      "category": "Math"
    }
  ]
  ```

#### Obtenir les questions d'un challenge
- **URL** : `GET /api/ChallengeQuizzies/{id}`
- **Réponse** :
  ```json
  {
    "id": 1,
    "title": "Challenge 1",
    "questions": [
      {
        "id": 101,
        "title": "Question 1",
        "type": "Multiple Choice",
        "options": [
          {"id": 1, "text": "Option A"},
          {"id": 2, "text": "Option B"}
        ]
      }
    ]
  }
  ```

---

### Vérification des réponses

#### Vérifier une réponse
- **URL** : `POST /api/AnswerVerification/verify`
- **Body** :
  ```json
  {
    "questionId": 101,
    "selectedOptionId": 2
  }
  ```
- **Réponse** : `true` ou `false`

---

## Notes supplémentaires

- Les identifiants pour les utilisateurs et équipes sont au format GUID.
- Les mots de passe doivent respecter une politique stricte (longueur, caractères spéciaux, etc.).