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

form.addEventListener("submit", async function (event) {
    event.preventDefault();

    const reservationDate =
        document.getElementById("reservation-date").value;

    const reservationTime =
        document.getElementById("reservation-time").value;

    const numberOfGuests =
        document.getElementById("number-of-guests").value;

    const reservationStart = new Date(
        `${reservationDate}T${reservationTime}`
    );

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

        availabilityResult.textContent =
            `Brak wolnych stolików w wybranym terminie. ` +
            `Najbliższy dostępny termin: ${formattedDate}, ` +
            `${formattedStartTime}–${formattedEndTime}, ` +
            `stolik nr ${nextAvailableReservation.tableNumber}.`;

        return;
    }

    for (const table of availableTables) {
        const tableElement = document.createElement("p");

        tableElement.textContent =
            `Stolik nr ${table.tableNumber} — ${table.seats} miejsc`;

        availabilityResult.appendChild(tableElement);
    }

    console.log("Start:", reservationStart);
    console.log("Koniec:", reservationEnd);
    console.log("Goście:", guests);
});
