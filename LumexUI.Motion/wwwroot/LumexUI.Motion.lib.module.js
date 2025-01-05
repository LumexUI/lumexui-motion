import { animate as motionAnimate } from "https://cdn.jsdelivr.net/npm/motion@11.11.13/+esm"

async function animate(el, props, transition) {
    try {
        if (!(el instanceof HTMLElement)) {
            console.error("Invalid element provided");
        }

        const { enter } = props;

        await motionAnimate(el, enter, transition);
    } catch (error) {
        console.error("Animation failed:", error);
    }
}

window['motionInterop'] = {
    animate
}