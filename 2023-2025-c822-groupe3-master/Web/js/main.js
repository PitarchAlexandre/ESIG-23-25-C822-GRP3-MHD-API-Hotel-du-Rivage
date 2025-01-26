/**
 * @author Maëlle Huguenin-Dezot & Alexandre Pitarch
 * @since 2024.06.19
 */

'use strict';

// Va chercher le formulaire
let form = document.querySelector('form')
// Va rechercher les éléments du formulaire
let nom = document.querySelector('#nom');
let prenom = document.querySelector('#prenom');
let rue = document.querySelector('#rue');
let localite = document.querySelector('#localite');
let telephone = document.querySelector('#telephone');
let email = document.querySelector('#email');
let dateNaissance = document.querySelector('#dateNaissance');

/**
 * Ecoute l'évènement qui envoie le formulaire
 */
form.addEventListener('submit', sendFormBD);

/**
 * Fonction qui controle bien que tous les champs ont bien été remplis
 * par l'utilisateur
 */
function inputCheckUp() {
    // Controle que les champs nom et prénom sont remplis
    if (nom.value.trim() === '' || prenom.value.trim() === '') {
        alert('Veuillez saisir un nom et prénom.')
        return false;
    };

    // Controle que la rue et la localité sont insérées
    if (rue.value.trim() === '' || localite.value.trim() === '') {
        alert('Veuillez insérer l\'adresse et le nom de la localité ou vous habitez.');
        return false
    };

    // Controle que le champ téléphone soit rempli
    if (telephone.value.trim() === '' || telephone.value.trim().length === 8) {
        alert('Veuillez insérer une numéro de téléphone valide.');
        return false;
    };

    // Controle que l'utilisateur a inséré une adresse mail
    if (email.value.trim() === ''){
        alert('Veuillez insérer une adresse email valide.');
        return false;
    };

    // Controle que la date de naissance a été inséré
    if (dateNaissance.value.trim() === ''){
        alert('Veuilez insérer une date de naissance valide.');
        return false;
    };

    return true;
}

/**
 * Fonction qui controle que tous les éléments du formulaire sont remplis et valides
 * et envoie information à la base de donnée Infomaniak ci-dessous :
 * host: ih1o2.myd.infomaniak.com
 *
 */
function sendFormBD(e) {
    // Variable permettant de sortir de la fonction en cas d'erreur
    let continuer = true;
    // Fonction permettant de controler que tous les champs soient valides
     continuer = inputCheckUp();
    if (continuer === true){
        alert('Vos informations ont bien été enregistrées,' +
            ' vous allez être contacté par l\'un de nos collaborateurs.');
        // Envoie le formulaire un message et vide les champs
        form.submit();
        form.reset();
    } else {
        // Désactive l'envoi du formulaire
        e.preventDefault();
    };
};