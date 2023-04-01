
function PresentErrorCardPopUp(errorMessage) {
    // Select the error card section
    const errorCardSection = document.querySelector('#error_card_section');

    // Create a new paragraph element with the error message
    const cardDiv = document.createElement('div');
    cardDiv.classList.add('card card-body border-danger');

    const pMessage = document.createElement('p');
    pMessage.classList.add('text-danger');
    pMessage.textContent = errorMessage;

    // Append the error message to the error card section
    cardDiv.appendChild(pMessage);
    errorCardSection.appendChild(cardDiv);
}