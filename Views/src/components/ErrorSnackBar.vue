<template>
	<v-snackbar
		v-model="snackbar"
		:timeout="timeout"
		color="error"
		elevation="0"
		:right="true"
		:shaped="true"
		transition="slide-x-transition"
	>
		{{ errorNotificationText }}
		<template v-slot:action="{ attrs }">
			<v-btn color="blue" text v-bind="attrs" @click="snackbar = !snackbar">
				<v-icon large color="white">error</v-icon>
			</v-btn>
		</template>
	</v-snackbar>
</template>
<script>
	export default {
		name: "ErrorSnackBar",
		props: ["errorNotificationText"],

		data: () => ({
            timeout: 3500,
            snackbar: true,
		}),
	};
</script>
