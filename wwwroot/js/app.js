let selectedReservation = null;
function formatDateTimeForApi(date) {
    const year = date.getFullYear();

    const month = String(date.getMonth() + 1)
        .padStart(2, "0");

    const day = String(date.getDate())
        .padStart(2, "0");

    const hours = String(date.getHours())
        .padStart(2, "0");

    const minutes = String(date.getMinutes())
        .padStart(2, "0");

    const seconds = String(date.getSeconds())
        .padStart(2, "0");

    return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
}

const form = document.getElementById("availability-form");
const availabilityResult = document.getElementById("availability-result");
const reservationSection = document.getElementById("reservation-section");
const reservationForm = document.getElementById("reservation-form");

form.addEventListener("submit", async function (event) {
    event.preventDefault();

    const reservationDate =
        document.getElementById("reservation-date").value;

    const reservationTime =
        document.getElementById("reservation-time").value;

    const numberOfGuests =
        document.getElementById("number-of-guests").value;

    const reservationStart = new Date(
        `${reservationDate}T${reservationTime}`);

    const reservationEnd = new Date(reservationStart);
    reservationEnd.setHours(reservationEnd.getHours() + 2);

    const reservationStartForApi =
        `${reservationDate}T${reservationTime}:00`;

    const reservationEndForApi =
        formatDateTimeForApi(reservationEnd);

    const guests = Number(numberOfGuests);

    const parameters = new URLSearchParams({
        ReservationStart: reservationStartForApi,
        ReservationEnd: reservationEndForApi,
        NumberOfGuests: guests
    });

    availabilityResult.replaceChildren();

    const response = await fetch(
        `/api/table/available?${parameters}`
    );



    if (!response.ok) {
        console.error(
            "Błąd odpowiedzi API:",
            response.status
        );

        return;
    }

    const availableTables = await response.json();

    console.log(
        "Dostępne stoliki:",
        availableTables
    );


    if (availableTables.length === 0) {
        availabilityResult.textContent =
            "Brak wolnych stolików w wybranym terminie. Szukam najbliższego terminu...";

        const nextAvailableResponse = await fetch(
            `/api/reservation/nextavailable?${parameters}`
        );

        if (!nextAvailableResponse.ok) {
            availabilityResult.textContent =
                "Nie znaleziono wolnego terminu w ciągu najbliższych 7 dni.";

            return;
        }

        const nextAvailableReservation =
            await nextAvailableResponse.json();

        const nextStart =
            new Date(nextAvailableReservation.reservationStart);

        const nextEnd =
            new Date(nextAvailableReservation.reservationEnd);

        const formattedDate =
            nextStart.toLocaleDateString("pl-PL");

        const formattedStartTime =
            nextStart.toLocaleTimeString("pl-PL", {
                hour: "2-digit",
                minute: "2-digit"
            });

        const formattedEndTime =
            nextEnd.toLocaleTimeString("pl-PL", {
                hour: "2-digit",
                minute: "2-digit"
            });

        const suggestionText = document.createElement("p");

        suggestionText.textContent =
            `Najbliższy dostępny termin: ${formattedDate}, ` +
            `${formattedStartTime}–${formattedEndTime}, ` +
            `stolik nr ${nextAvailableReservation.tableNumber}.`;

        const selectSuggestionButton = document.createElement("button");
        selectSuggestionButton.type = "button";
        selectSuggestionButton.textContent = "Wybierz ten termin";

        selectSuggestionButton.addEventListener("click", function () {
            selectedReservation = {
                tableId: nextAvailableReservation.tableId,
                reservationStart: nextAvailableReservation.reservationStart,
                reservationEnd: nextAvailableReservation.reservationEnd,
                numberOfGuests: guests
            };
            reservationSection.hidden = false;

            const selectedReservationSummary = document.getElementById("selected-reservation-summary");
            console.log("Wybrana sugerowana rezerwacja:", selectedReservation);
        });

        availabilityResult.replaceChildren(suggestionText,selectSuggestionButton);

        return;
    }

    for (const table of availableTables) {
        const tableElement = document.createElement("p");

        tableElement.textContent =
            `Stolik nr ${table.tableNumber} — ${table.seats} miejsc`;

        const selectButton = document.createElement("button");
        selectButton.type = "button";
        selectButton.textContent = "Wybierz";

        selectButton.addEventListener("click", function () {
            selectedReservation = {
                tableId: table.id,
                reservationStart: reservationStartForApi,
                reservationEnd: reservationEndForApi,
                numberOfGuests: guests
            
            };
            reservationSection.hidden = false;

            console.log("Wybrana rezerwacja:", selectedReservation);


        });

        availabilityResult.appendChild(tableElement);
        availabilityResult.appendChild(selectButton);
    }

    console.log("Start:", reservationStart);
    console.log("Koniec:", reservationEnd);
    console.log("Goście:", guests);
})
reservationForm.addEventListener("submit", async function (event) {
    event.preventDefault();

    if (selectedReservation === null) {
        console.error("Nie wybrano rezerwacji.");
        return;
    }

    const customerName =
        document.getElementById("customer-name").value.trim();

    const phoneNumber =
        document.getElementById("phone-number").value.trim();

    const notes =
        document.getElementById("notes").value.trim();

    const reservation = {
        customerName: customerName,
        phoneNumber: phoneNumber,
        reservationStart: selectedReservation.reservationStart,
        reservationEnd: selectedReservation.reservationEnd,
        numberOfGuests: selectedReservation.numberOfGuests,
        notes: notes,
        tableId: selectedReservation.tableId
    };

    console.log("Rezerwacja do wysłania:", reservation);

    const response = await fetch("/api/reservation", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(reservation)

    });
    if (!response.ok) {
        console.error(
            "Nie udalo sie utworzyc rezerwacji:",
            response.status
        );
        return;
    }
    console.log("Rezerwacja zostala utworzona.");

    availabilityResult.textContent = "Rezerwacja została pomyślnie utworzona!";

    reservationForm.reset();
    reservationSection.hidden = true;
    selectedReservation = null;
});
;
