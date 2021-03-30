jsSocials.setDefaults("twitter", {
    logo: "fab fa-twitter"
});
jsSocials.setDefaults("facebook", {
    logo: "fab fa-facebook-f",
    label: "Share"
});
jsSocials.setDefaults("linkedin", {
    logo: "fab fa-linkedin-in"
});

$("#share-on-social-media").jsSocials({
    shares: [
        "twitter",
        "facebook",
        "linkedin"
    ],
    text: "Ik las dit artikel op",
    shareIn: "popup"
});
