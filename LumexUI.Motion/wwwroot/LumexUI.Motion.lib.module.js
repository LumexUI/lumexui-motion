import { animate as motionAnimate } from "https://cdn.jsdelivr.net/npm/motion@11.11.13/+esm"

const layoutRegistry = {};

async function animateEnter(ref, props) {
    try {
        await animateCore(ref, props, "enter");
    } catch (error) {
        console.error("`animateEnter` failed:", error);
    }
}

async function animateExit(ref, props) {
    try {
        await animateCore(ref, props, "exit");
    } catch (error) {
        console.error("`animateExit` failed:", error);
    }
}

async function animateLayoutId(ref, layoutId, props) {
    try {
        if (!(ref instanceof HTMLElement)) {
            throw new Error("Invalid element provided");
        }

        const rect = ref.getBoundingClientRect();
        const curr = { ref, rect };

        // If we already have a stored rect for this layoutId,
        // animate from the prev -> curr.
        if (layoutRegistry[layoutId]) {
            const prev = layoutRegistry[layoutId];
            const deltaX = prev.rect.x - curr.rect.x;
            const enter = {
                x: [deltaX, 0]
            };

            props = mergeDeep(props || {}, { enter });

            // Update the stored rect
            layoutRegistry[layoutId] = curr;

            if (ref != prev.ref) {
                // Animate from prev => curr
                await animateEnter(ref, props);
            }
        } else {
            // First time we see this layoutId, store it
            layoutRegistry[layoutId] = curr;
        }
    } catch (error) {
        console.error("`animateLayoutId` failed:", error);
    }
}

async function animateCore(ref, props, key) {
    if (!(ref instanceof HTMLElement)) {
        throw new Error("Invalid element provided");
    }

    const { transition } = props || {};
    const animationProps = props?.[key];

    await motionAnimate(ref, animationProps, transition);
}

function isObject(item) {
    return item && typeof item === "object" && !Array.isArray(item);
}

function mergeDeep(target, ...sources) {
    if (!sources.length) return target;
    const source = sources.shift();

    if (isObject(target) && isObject(source)) {
        for (const key in source) {
            if (isObject(source[key])) {
                if (!target[key]) Object.assign(target, { [key]: {} });
                mergeDeep(target[key], source[key]);
            } else {
                Object.assign(target, { [key]: source[key] });
            }
        }
    }

    return mergeDeep(target, ...sources);
}

window['motionInterop'] = {
    animateEnter,
    animateExit,
    animateLayoutId
}