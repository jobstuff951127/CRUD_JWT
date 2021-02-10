<template>
	<v-snackbar
		v-model="snackbar"
		:timeout="timeout"
		color="green darken-3"
		elevation="0"
		:right="true"
		:shaped="true"
		transition="slide-x-transition"
	>
		{{ success }}
		<template v-slot:action="{ attrs }">
			<v-btn color="blue" text v-bind="attrs" @click="snackbar = !snackbar">
				<v-icon large color="white">done</v-icon>
			</v-btn>
		</template>
	</v-snackbar>
</template>
<script>
	export default {
		name: "SuccessSnackBar",
		props: ["success"],

		data: () => ({
			timeout: 2000,
			snackbar: true,
		}),
	};
</script>
