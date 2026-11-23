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

    console.log("Start:", reservationStart);
    console.log("Koniec:", reservationEnd);
    console.log("Goście:", guests);
});
